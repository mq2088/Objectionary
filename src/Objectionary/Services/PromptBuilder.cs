using System.Text;
using Objectionary.Data;

namespace Objectionary.Services;

public static class PromptBuilder
{
    public static string BuildUserMessage(
        string[] concerns,
        string tone,
        string? personalNote,
        string variationSeed)
    {
        var sb = new StringBuilder();

        var today = DateTime.UtcNow.AddHours(10); // AEST
        sb.AppendLine($"TODAY'S DATE: {today:dd MMMM yyyy}");
        sb.AppendLine();
        sb.AppendLine($"TONE: {tone}");
        sb.AppendLine();
        sb.AppendLine($"VARIATION: {variationSeed}");
        sb.AppendLine();
        sb.AppendLine("SELECTED CONCERNS:");
        sb.AppendLine();

        foreach (var concernId in concerns)
        {
            if (Concerns.Details.TryGetValue(concernId, out var info))
            {
                sb.AppendLine($"### {info.Title}");
                sb.AppendLine(info.Description);
                sb.AppendLine($"Relevant planning instruments: {info.PlanningInstruments}");
                sb.AppendLine();
            }
        }

        if (!string.IsNullOrEmpty(personalNote))
        {
            sb.AppendLine("PERSONAL NOTE (MANDATORY — you MUST include this in the letter as its own paragraph or woven into the body):");
            sb.AppendLine(personalNote);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
