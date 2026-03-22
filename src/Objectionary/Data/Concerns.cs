namespace Objectionary.Data;

public static class Concerns
{
    public static readonly HashSet<string> ValidIds = new(StringComparer.OrdinalIgnoreCase)
    {
        "height_bulk",
        "structural_risk",
        "heritage",
        "scenic_protection",
        "traffic",
        "infrastructure",
        "non_compliance",
        "affordable_housing"
    };

    public static readonly Dictionary<string, ConcernInfo> Details = new(StringComparer.OrdinalIgnoreCase)
    {
        ["height_bulk"] = new(
            "Excessive Height and Bulk",
            "The proposal is for a 10-storey building, which is clearly out of scale with the surrounding low-rise context.",
            "Mosman LEP 2012 Cl 4.3 (Height), Cl 4.4 (FSR)"
        ),
        ["structural_risk"] = new(
            "Overdevelopment and Structural Risk",
            "The development requires excavation of up to 10 metres into sandstone, extending to site boundaries. This creates real risks of ground movement, vibration and damage to neighbouring properties. The site is being engineered to accommodate the building, rather than the building responding to the site.",
            "EP&A Act s4.15(1)(b) — likely impacts"
        ),
        ["heritage"] = new(
            "Heritage Impacts",
            "The site adjoins heritage listed properties at 36 and 38 Redan Street. The proposal would overwhelm these heritage items, disrupt their visual setting and erode the existing character of Redan Street.",
            "Mosman LEP 2012 Cl 5.10 (Heritage), heritage listing of 36 & 38 Redan St"
        ),
        ["scenic_protection"] = new(
            "Scenic Protection Area Conflict",
            "The site is affected by a Scenic Protection Area, where planning objectives include limiting visual intrusion and protecting landscape character. A 10-storey building is difficult to reconcile with those objectives.",
            "Mosman LEP 2012 Cl 6.5 (Scenic Protection Area)"
        ),
        ["traffic"] = new(
            "Traffic and Access Concerns",
            "Safety concerns arise from the use of Redan Lane, which is only a little over 4 metres wide, has no footpaths, and is not designed for increased service and waste vehicle activity. The introduction of larger vehicles into such a constrained space raises clear safety and access issues.",
            "EP&A Act s4.15(1)(e) — public interest, RMS guidelines"
        ),
        ["infrastructure"] = new(
            "Lack of Supporting Infrastructure",
            "There are growing concerns that infrastructure is not keeping pace with the scale of development being proposed. Mosman currently operates with a single fire station and limited emergency services capacity. A significant increase in population, combined with the introduction of 10-storey buildings, raises practical questions about emergency response, access for large fire vehicles, and overall service capacity in narrow residential streets.",
            "EP&A Act s4.15(1)(c) — suitability of site, s4.15(1)(e) — public interest"
        ),
        ["non_compliance"] = new(
            "Non-Compliance and Misleading Claims",
            "The proposal exceeds height controls and requires a Clause 4.6 variation, yet is described as \"compliant\". A development that breaches fundamental controls should not be presented as compliant.",
            "Cl 4.6 variation provisions, Mosman LEP 2012 development standards"
        ),
        ["affordable_housing"] = new(
            "Affordable Housing Design and Access Concerns",
            "The proposal includes 11 affordable housing units with separate access from the laneway, which raises concerns about a \"poor door\" style arrangement and whether the design supports inclusive housing.",
            "SEPP (Housing) 2021, inclusive design principles"
        )
    };
}

public record ConcernInfo(string Title, string Description, string PlanningInstruments);
