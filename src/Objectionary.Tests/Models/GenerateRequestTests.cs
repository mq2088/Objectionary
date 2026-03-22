using System.ComponentModel.DataAnnotations;
using Objectionary.Models;

namespace Objectionary.Tests.Models;

public class GenerateRequestTests
{
    [Fact]
    public void Valid_request_passes_validation()
    {
        var request = new GenerateRequest
        {
            Concerns = ["height_bulk", "heritage"],
            Tone = "formal",
            PersonalNote = null
        };

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);

        Assert.True(isValid);
    }

    [Fact]
    public void Empty_concerns_fails_validation()
    {
        var request = new GenerateRequest
        {
            Concerns = [],
            Tone = "formal"
        };

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void Invalid_concern_value_fails_validation()
    {
        var request = new GenerateRequest
        {
            Concerns = ["height_bulk", "not_a_real_concern"],
            Tone = "formal"
        };

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void Invalid_tone_fails_validation()
    {
        var request = new GenerateRequest
        {
            Concerns = ["height_bulk"],
            Tone = "aggressive"
        };

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void PersonalNote_over_300_chars_fails_validation()
    {
        var request = new GenerateRequest
        {
            Concerns = ["height_bulk"],
            Tone = "formal",
            PersonalNote = new string('a', 301)
        };

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), results, true);

        Assert.False(isValid);
    }
}
