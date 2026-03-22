using System.ComponentModel.DataAnnotations;
using Objectionary.Data;

namespace Objectionary.Models;

public class GenerateRequest : IValidatableObject
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one concern must be selected.")]
    public string[] Concerns { get; set; } = [];

    [Required]
    public string Tone { get; set; } = "formal";

    [MaxLength(300)]
    public string? PersonalNote { get; set; }

    private static readonly HashSet<string> ValidTones = new(StringComparer.OrdinalIgnoreCase)
    {
        "formal", "moderate", "personal"
    };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ValidTones.Contains(Tone))
            yield return new ValidationResult($"Invalid tone: {Tone}. Must be formal, moderate, or personal.");

        foreach (var concern in Concerns)
        {
            if (!Data.Concerns.ValidIds.Contains(concern))
                yield return new ValidationResult($"Invalid concern: {concern}.");
        }
    }
}
