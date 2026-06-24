using CyberCareerFitAnalyzer.Models;

namespace CyberCareerFitAnalyzer.Services;

public class AssessmentEngine
{
    public AssessmentResult AssessJob(JobPosting job, List<string> userSkills)
    {
        List<string> normalizedUserSkills = userSkills
            .Select(skill => skill.Trim().ToLower())
            .ToList();

        List<string> matchedSkills = job.RequiredSkills
            .Concat(job.PreferredSkills)
            .Where(skill => normalizedUserSkills.Contains(skill.ToLower()))
            .Distinct()
            .ToList();

        List<string> missingRequiredSkills = job.RequiredSkills
            .Where(skill => !normalizedUserSkills.Contains(skill.ToLower()))
            .ToList();

        int totalSkillCount = job.RequiredSkills.Count + job.PreferredSkills.Count;
        int fitScore = totalSkillCount == 0
            ? 0
            : (int)Math.Round((matchedSkills.Count / (double)totalSkillCount) * 100);

        return new AssessmentResult
        {
            JobId = job.Id,
            JobTitle = job.JobTitle,
            Company = job.Company,
            FitScore = fitScore,
            MatchedSkills = matchedSkills,
            MissingRequiredSkills = missingRequiredSkills,
            SuggestedTalkingPoints = GenerateTalkingPoints(job, matchedSkills, missingRequiredSkills)
        };
    }

    private List<string> GenerateTalkingPoints(
        JobPosting job,
        List<string> matchedSkills,
        List<string> missingRequiredSkills)
    {
        List<string> talkingPoints = new();

        if (matchedSkills.Any())
        {
            talkingPoints.Add("Emphasize hands-on experience with: " + string.Join(", ", matchedSkills));
        }

        if (missingRequiredSkills.Any())
        {
            talkingPoints.Add("Prepare a growth-focused answer for missing skills: " + string.Join(", ", missingRequiredSkills));
        }

        if (job.JobTitle.ToLower().Contains("cyber") || job.JobTitle.ToLower().Contains("security"))
        {
            talkingPoints.Add("Discuss secure coding, risk awareness, incident response, and defensive thinking.");
        }

        if (job.JobTitle.ToLower().Contains("software") || job.JobTitle.ToLower().Contains("developer"))
        {
            talkingPoints.Add("Discuss clean code, debugging, version control, testing, and maintainable design.");
        }

        talkingPoints.Add("Prepare one STAR story showing problem-solving, persistence, and technical learning.");

        return talkingPoints;
    }
}