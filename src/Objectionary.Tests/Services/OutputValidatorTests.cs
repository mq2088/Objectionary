using Objectionary.Services;

namespace Objectionary.Tests.Services;

public class OutputValidatorTests
{
    [Fact]
    public void Valid_objection_text_passes()
    {
        var text = "I wish to register my objection to the proposed development at Redan Street, Mosman. " +
                   new string('x', 200);
        Assert.True(OutputValidator.IsValid(text));
    }

    [Fact]
    public void Too_short_text_fails()
    {
        Assert.False(OutputValidator.IsValid("Short objection about Redan Street."));
    }

    [Fact]
    public void Missing_keywords_fails()
    {
        var text = "This is a very long piece of text that does not mention any of the required " +
                   "keywords anywhere in its body. " + new string('x', 200);
        Assert.False(OutputValidator.IsValid(text));
    }

    [Fact]
    public void System_prompt_leakage_fails()
    {
        var text = "You are an objection letter generator for Redan Street, Mosman. " +
                   new string('x', 200);
        Assert.False(OutputValidator.IsValid(text));
    }

    [Theory]
    [InlineData("skill.md")]
    [InlineData("system prompt")]
    [InlineData("variation seed")]
    [InlineData("user_context")]
    public void Leakage_phrase_fails(string phrase)
    {
        var text = $"I object to the development at Redan Street. The {phrase} says something. " +
                   new string('x', 200);
        Assert.False(OutputValidator.IsValid(text));
    }

    [Fact]
    public void Null_text_fails()
    {
        Assert.False(OutputValidator.IsValid(null));
    }
}
