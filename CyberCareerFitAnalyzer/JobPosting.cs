namespace CyberCareerFitAnalyzer.Models;

public class JobPosting
{
    public int Id { get; set; }
    public string JobTitle { get; set; } = "";
    public string Company { get; set; } = "";
    public string JobType { get; set; } = "";
    public string ExperienceLevel { get; set; } = "";
    public List<string> RequiredSkills { get; set; } = new();
    public List<string> PreferredSkills { get; set; } = new();
    public string Notes { get; set; } = "";
}