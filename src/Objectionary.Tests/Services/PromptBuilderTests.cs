using Objectionary.Services;

namespace Objectionary.Tests.Services;

public class PromptBuilderTests
{
    [Fact]
    public void Builds_message_with_concerns_and_tone()
    {
        var message = PromptBuilder.BuildUserMessage(
            concerns: ["height_bulk", "heritage"],
            tone: "formal",
            personalNote: null,
            variationSeed: "Open with a question about neighbourhood character"
        );

        Assert.Contains("Excessive Height and Bulk", message);
        Assert.Contains("Heritage Impacts", message);
        Assert.Contains("formal", message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Open with a question about neighbourhood character", message);
    }

    [Fact]
    public void Includes_personal_note_in_delimited_block()
    {
        var message = PromptBuilder.BuildUserMessage(
            concerns: ["traffic"],
            tone: "personal",
            personalNote: "I live nearby and worry about safety",
            variationSeed: "Lead with the heritage impact"
        );

        Assert.Contains("<user_context>", message);
        Assert.Contains("I live nearby and worry about safety", message);
        Assert.Contains("</user_context>", message);
    }

    [Fact]
    public void Excludes_user_context_block_when_note_is_null()
    {
        var message = PromptBuilder.BuildUserMessage(
            concerns: ["traffic"],
            tone: "formal",
            personalNote: null,
            variationSeed: "Lead with the heritage impact"
        );

        Assert.DoesNotContain("<user_context>", message);
    }

    [Fact]
    public void Includes_planning_instruments()
    {
        var message = PromptBuilder.BuildUserMessage(
            concerns: ["height_bulk"],
            tone: "formal",
            personalNote: null,
            variationSeed: "test seed"
        );

        Assert.Contains("Mosman LEP 2012 Cl 4.3", message);
    }
}
