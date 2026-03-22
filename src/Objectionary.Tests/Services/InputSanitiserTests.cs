using Objectionary.Services;

namespace Objectionary.Tests.Services;

public class InputSanitiserTests
{
    [Fact]
    public void Null_input_returns_null()
    {
        Assert.Null(InputSanitiser.Sanitise(null));
    }

    [Fact]
    public void Empty_input_returns_null()
    {
        Assert.Null(InputSanitiser.Sanitise(""));
    }

    [Fact]
    public void Clean_text_passes_through()
    {
        var input = "I live two doors down from the site.";
        Assert.Equal(input, InputSanitiser.Sanitise(input));
    }

    [Fact]
    public void Allowed_special_chars_preserved()
    {
        var input = "my children (aged 5 & 7) walk past; it's unsafe/dangerous.";
        Assert.Equal(input, InputSanitiser.Sanitise(input));
    }

    [Fact]
    public void Disallowed_chars_stripped()
    {
        var input = "Hello <script>alert('xss')</script> world";
        Assert.Equal("Hello scriptalert('xss')/script world", InputSanitiser.Sanitise(input));
    }

    [Fact]
    public void Truncates_to_300_chars()
    {
        var input = new string('a', 350);
        var result = InputSanitiser.Sanitise(input);
        Assert.Equal(300, result!.Length);
    }

    [Fact]
    public void Whitespace_only_returns_null()
    {
        Assert.Null(InputSanitiser.Sanitise("   "));
    }
}
