namespace Objectionary.Data;

public static class VariationSeeds
{
    private static readonly string[] Seeds =
    [
        "Open with a question about what this development means for the neighbourhood's character",
        "Lead with the heritage impact and how the proposal threatens adjacent listed properties",
        "Begin by referencing the relevant planning framework and how the proposal fails to meet it",
        "Start with the personal and community impact on residents who live nearby",
        "Open with the stark scale contrast between the 10-storey proposal and the 2-3 storey surroundings",
        "Begin with the infrastructure and emergency services concerns this development raises",
        "Open by questioning the credibility of the application given its non-compliance claims",
        "Lead with the environmental and scenic protection implications",
        "Start with the traffic and pedestrian safety risks posed by construction and ongoing use",
        "Open with a statement about the cumulative impact of overdevelopment in the area",
        "Begin by addressing the structural risks of deep sandstone excavation near existing homes",
        "Lead with the affordable housing design concerns and what inclusive housing should look like",
        "Open with a reflection on what Mosman's planning controls are designed to protect",
        "Start by noting the tension between state significant development powers and local planning objectives",
        "Begin with the precedent this approval would set for future development in the area",
        "Lead with concerns about the adequacy of the environmental and traffic assessments provided",
        "Open with observations about the site's constraints and why they make this proposal unsuitable",
        "Start with the community's perspective on how this development will change the streetscape",
        "Begin by highlighting the gap between the development's marketed benefits and its actual impacts",
        "Lead with the question of whether Clause 4.6 variation is justified for a breach of this magnitude"
    ];

    public static string GetRandom() => Seeds[Random.Shared.Next(Seeds.Length)];
}
