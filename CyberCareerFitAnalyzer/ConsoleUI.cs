using CyberCareerFitAnalyzer.Models;
using CyberCareerFitAnalyzer.Services;

namespace CyberCareerFitAnalyzer.UI;

public class ConsoleUI
{
    private readonly JobManager _jobManager = new();
    private readonly AssessmentEngine _assessmentEngine = new();

    public void Run()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            ShowHeader();

            Console.WriteLine("1. Add job posting");
            Console.WriteLine("2. View saved jobs");
            Console.WriteLine("3. Analyze job fit");
            Console.WriteLine("4. Delete job");
            Console.WriteLine("5. Exit");
            Console.Write("\nChoose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddJob();
                    break;
                case "2":
                    ViewJobs();
                    break;
                case "3":
                    AnalyzeJob();
                    break;
                case "4":
                    DeleteJob();
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Pause("Invalid choice.");
                    break;
            }
        }
    }

    private void ShowHeader()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("      CYBER CAREER FIT ANALYZER");
        Console.WriteLine("=======================================\n");
    }

    private void AddJob()
    {
        Console.Clear();
        ShowHeader();

        JobPosting job = new();

        Console.Write("Job title: ");
        job.JobTitle = Console.ReadLine() ?? "";

        Console.Write("Company: ");
        job.Company = Console.ReadLine() ?? "";

        Console.Write("Job type: ");
        job.JobType = Console.ReadLine() ?? "";

        Console.Write("Experience level: ");
        job.ExperienceLevel = Console.ReadLine() ?? "";

        Console.Write("Required skills, separated by commas: ");
        job.RequiredSkills = ParseSkillList(Console.ReadLine());

        Console.Write("Preferred skills, separated by commas: ");
        job.PreferredSkills = ParseSkillList(Console.ReadLine());

        Console.Write("Notes: ");
        job.Notes = Console.ReadLine() ?? "";

        _jobManager.AddJob(job);

        Pause("Job saved successfully.");
    }

    private void ViewJobs()
    {
        Console.Clear();
        ShowHeader();

        List<JobPosting> jobs = _jobManager.GetAllJobs();

        if (!jobs.Any())
        {
            Pause("No jobs saved yet.");
            return;
        }

        foreach (JobPosting job in jobs)
        {
            Console.WriteLine($"ID: {job.Id}");
            Console.WriteLine($"Title: {job.JobTitle}");
            Console.WriteLine($"Company: {job.Company}");
            Console.WriteLine($"Type: {job.JobType}");
            Console.WriteLine($"Level: {job.ExperienceLevel}");
            Console.WriteLine($"Required: {string.Join(", ", job.RequiredSkills)}");
            Console.WriteLine($"Preferred: {string.Join(", ", job.PreferredSkills)}");
            Console.WriteLine($"Notes: {job.Notes}");
            Console.WriteLine("---------------------------------------");
        }

        Pause();
    }

    private void AnalyzeJob()
    {
        Console.Clear();
        ShowHeader();

        ViewJobSummary();

        Console.Write("\nEnter job ID to analyze: ");
        bool validId = int.TryParse(Console.ReadLine(), out int id);

        if (!validId)
        {
            Pause("Invalid job ID.");
            return;
        }

        JobPosting? job = _jobManager.GetJobById(id);

        if (job == null)
        {
            Pause("Job not found.");
            return;
        }

        Console.WriteLine("\nEnter your current skills separated by commas.");
        Console.WriteLine("Example: C#, SQL, Git, Security+, Risk Assessment, JSON");
        Console.Write("\nYour skills: ");

        List<string> userSkills = ParseSkillList(Console.ReadLine());

        AssessmentResult result = _assessmentEngine.AssessJob(job, userSkills);

        Console.Clear();
        ShowHeader();

        Console.WriteLine($"Assessment for: {result.JobTitle} at {result.Company}");
        Console.WriteLine($"Fit Score: {result.FitScore}%");

        Console.WriteLine("\nMatched Skills:");
        PrintList(result.MatchedSkills);

        Console.WriteLine("\nMissing Required Skills:");
        PrintList(result.MissingRequiredSkills);

        Console.WriteLine("\nInterview Talking Points:");
        PrintList(result.SuggestedTalkingPoints);

        Pause();
    }

    private void DeleteJob()
    {
        Console.Clear();
        ShowHeader();

        ViewJobSummary();

        Console.Write("\nEnter job ID to delete: ");
        bool validId = int.TryParse(Console.ReadLine(), out int id);

        if (!validId)
        {
            Pause("Invalid job ID.");
            return;
        }

        bool deleted = _jobManager.DeleteJob(id);

        Pause(deleted ? "Job deleted." : "Job not found.");
    }

    private void ViewJobSummary()
    {
        List<JobPosting> jobs = _jobManager.GetAllJobs();

        if (!jobs.Any())
        {
            Console.WriteLine("No jobs saved yet.");
            return;
        }

        foreach (JobPosting job in jobs)
        {
            Console.WriteLine($"{job.Id}. {job.JobTitle} - {job.Company}");
        }
    }

    private List<string> ParseSkillList(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<string>();
        }

        return input
            .Split(',')
            .Select(skill => skill.Trim())
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .ToList();
    }

    private void PrintList(List<string> items)
    {
        if (!items.Any())
        {
            Console.WriteLine("- None");
            return;
        }

        foreach (string item in items)
        {
            Console.WriteLine($"- {item}");
        }
    }

    private void Pause(string message = "Press Enter to continue.")
    {
        Console.WriteLine($"\n{message}");
        Console.ReadLine();
    }
}