namespace Objectionary.Services;

public static class OutputValidator
{
    private const int MinLength = 200;

    private static readonly string[] RequiredKeywords = ["Redan Street", "objection", "Mosman"];

    private static readonly string[] LeakagePhrases =
    [
        "You are an objection",
        "skill.md",
        "system prompt",
        "variation seed",
        "user_context"
    ];

    public static bool IsValid(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        if (text.Length < MinLength)
            return false;

        var hasKeyword = RequiredKeywords.Any(k =>
            text.Contains(k, StringComparison.OrdinalIgnoreCase));
        if (!hasKeyword)
            return false;

        var hasLeakage = LeakagePhrases.Any(p =>
            text.Contains(p, StringComparison.OrdinalIgnoreCase));
        if (hasLeakage)
            return false;

        return true;
    }
}
