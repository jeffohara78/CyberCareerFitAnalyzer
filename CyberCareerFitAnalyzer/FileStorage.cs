using System.Text.Json;
using CyberCareerFitAnalyzer.Models;

namespace CyberCareerFitAnalyzer.Services;

public class FileStorage
{
    private readonly string _filePath = "jobs.json";

    public List<JobPosting> LoadJobs()
    {
        if (!File.Exists(_filePath))
        { 
            return new List<JobPosting>();
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<JobPosting>();
        }

        return JsonSerializer.Deserialize<List<JobPosting>>(json) ?? new List<JobPosting>();
    }

    public void SaveJobs(List<JobPosting> jobs)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        string json = JsonSerializer.Serialize(jobs, options);
        File.WriteAllText(_filePath, json);
    }
}