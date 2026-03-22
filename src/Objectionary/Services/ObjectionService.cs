using Anthropic.SDK;
using Anthropic.SDK.Messaging;
using Objectionary.Data;

namespace Objectionary.Services;

public class ObjectionService
{
    private readonly AnthropicClient _client;
    private readonly string _systemPrompt;
    private readonly string _model;
    private readonly CircuitBreaker _circuitBreaker;
    private readonly SemaphoreSlim _concurrencyThrottle;
    private readonly ILogger<ObjectionService> _logger;
    private readonly int _timeoutSeconds;

    public ObjectionService(
        AnthropicClient client,
        CircuitBreaker circuitBreaker,
        IConfiguration config,
        ILogger<ObjectionService> logger)
    {
        _client = client;
        _circuitBreaker = circuitBreaker;
        _logger = logger;
        _model = config["Anthropic:Model"] ?? "claude-haiku-4-5-20251001";
        _timeoutSeconds = config.GetValue("Anthropic:TimeoutSeconds", 30);
        _concurrencyThrottle = new SemaphoreSlim(
            config.GetValue("Anthropic:MaxConcurrentCalls", 50));

        var skillPath = Path.Combine(AppContext.BaseDirectory, "skill.md");
        if (!File.Exists(skillPath))
            throw new FileNotFoundException($"skill.md not found at {skillPath}");
        _systemPrompt = File.ReadAllText(skillPath);
    }

    public async Task<string?> GenerateAsync(string[] concerns, string tone, string? personalNote)
    {
        if (!_circuitBreaker.TryAcquire())
        {
            _logger.LogWarning("Circuit breaker tripped at {Count} requests", _circuitBreaker.Count);
            throw new CircuitBreakerTrippedException();
        }

        var seed = VariationSeeds.GetRandom();
        var sanitisedNote = InputSanitiser.Sanitise(personalNote);
        var userMessage = PromptBuilder.BuildUserMessage(concerns, tone, sanitisedNote, seed);

        var result = await CallClaudeAsync(userMessage);

        if (result != null && OutputValidator.IsValid(result))
            return result;

        _logger.LogWarning("First generation failed validation, retrying with new seed");
        var retrySeed = VariationSeeds.GetRandom();
        var retryMessage = PromptBuilder.BuildUserMessage(concerns, tone, sanitisedNote, retrySeed);
        var retryResult = await CallClaudeAsync(retryMessage);

        if (retryResult != null && OutputValidator.IsValid(retryResult))
            return retryResult;

        _logger.LogError("Both generation attempts failed validation");
        return null;
    }

    private async Task<string?> CallClaudeAsync(string userMessage)
    {
        await _concurrencyThrottle.WaitAsync();
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_timeoutSeconds));

            var parameters = new MessageParameters
            {
                Model = _model,
                MaxTokens = 2048,
                Temperature = 0.8m,
                System = [new SystemMessage(_systemPrompt)],
                Messages = [new Message(RoleType.User, userMessage)]
            };

            var response = await _client.Messages.GetClaudeMessageAsync(parameters, cts.Token);
            return response?.Content?.OfType<TextContent>().FirstOrDefault()?.Text;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Claude API call timed out after {Timeout}s", _timeoutSeconds);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Claude API call failed");
            return null;
        }
        finally
        {
            _concurrencyThrottle.Release();
        }
    }
}

public class CircuitBreakerTrippedException : Exception
{
    public CircuitBreakerTrippedException()
        : base("Circuit breaker tripped — maximum request count exceeded.") { }
}
