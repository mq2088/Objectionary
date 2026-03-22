// src/Objectionary/Program.cs
using System.ComponentModel.DataAnnotations;
using System.Threading.RateLimiting;
using Anthropic.SDK;
using Objectionary.Models;
using Objectionary.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<AnthropicClient>(_ =>
    new AnthropicClient(builder.Configuration["Anthropic:ApiKey"]
        ?? Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")
        ?? throw new InvalidOperationException("Anthropic API key not configured")));

builder.Services.AddSingleton(sp =>
    new CircuitBreaker(builder.Configuration.GetValue("Anthropic:MaxRequestsTotal", 25000)));

builder.Services.AddSingleton<ObjectionService>();

// Rate limiting with structured 429 responses
// Two separate policies are used because AddChainedLimiter is not available in this SDK version.
// Both "burst" and "hourly" are applied to /api/generate via two RequireRateLimiting calls.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    // Custom OnRejected handler to produce structured JSON 429 responses
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.ContentType = "application/json";

        var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue)
            ? retryAfterValue
            : TimeSpan.Zero;

        var isBurst = retryAfter <= TimeSpan.FromMinutes(1);
        var error = new ErrorResponse(
            "rate_limited",
            isBurst ? "burst" : "hourly",
            isBurst
                ? "You've reached the limit of 3 per minute. Please wait a moment."
                : "You've reached the limit of 5 per hour. Please try again later."
        );

        await context.HttpContext.Response.WriteAsJsonAsync(error, cancellationToken);
    };

    // Burst limit: 3 requests per minute per IP
    options.AddPolicy("burst", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 3,
            Window = TimeSpan.FromMinutes(1)
        });
    });

    // Hourly limit: 5 requests per hour per IP
    options.AddPolicy("hourly", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetSlidingWindowLimiter(ip, _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromHours(1),
            SegmentsPerWindow = 6
        });
    });
});

// Request size limit
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 2048;
});

var app = builder.Build();

app.UseRateLimiter();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.CacheControl = "public, max-age=3600";
    }
});

// Health check
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy" }));

// Generate endpoint with validation and rate limiting
app.MapPost("/api/generate", async (GenerateRequest request, ObjectionService service) =>
{
    // Validate request (Minimal APIs don't auto-validate IValidatableObject)
    var validationResults = new List<ValidationResult>();
    if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
    {
        var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
        return Results.BadRequest(new { error = "validation_failed", message = errors });
    }

    try
    {
        var result = await service.GenerateAsync(request.Concerns, request.Tone, request.PersonalNote);

        if (result == null)
            return Results.Problem("Generation failed. Please try again.", statusCode: 500);

        return Results.Ok(new GenerateResponse(result));
    }
    catch (CircuitBreakerTrippedException)
    {
        return Results.Json(
            new ErrorResponse("circuit_breaker", null, "Generation temporarily unavailable."),
            statusCode: 503);
    }
})
.RequireRateLimiting("burst")
.RequireRateLimiting("hourly");

app.MapFallbackToFile("index.html");

app.Run();
