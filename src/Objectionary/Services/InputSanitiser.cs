using System.Text.RegularExpressions;

namespace Objectionary.Services;

public static partial class InputSanitiser
{
    private const int MaxLength = 300;

    [GeneratedRegex(@"[^a-zA-Z0-9 .,!?'\-():;&/]")]
    private static partial Regex DisallowedChars();

    public static string? Sanitise(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var cleaned = DisallowedChars().Replace(input, "");

        if (cleaned.Length > MaxLength)
            cleaned = cleaned[..MaxLength];

        return string.IsNullOrWhiteSpace(cleaned) ? null : cleaned;
    }
}
