namespace CyberCareerFitAnalyzer.Models;

public class AssessmentResult
{ 
    public int JobId { get; set; }
    public string JobTitle { get; set; } = "";
    public string Company { get; set; } = "";
    public int FitScore { get; set; }
    public List<string> MatchedSkills { get; set; } = new();
    public List<string> MissingRequiredSkills { get; set; } = new();
    public List<string> SuggestedTalkingPoints { get; set; } = new();
}