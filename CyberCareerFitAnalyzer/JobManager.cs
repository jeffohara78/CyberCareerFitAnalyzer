using CyberCareerFitAnalyzer.Models;

namespace CyberCareerFitAnalyzer.Services;

public class JobManager
{
    private readonly FileStorage _storage = new();
    private readonly List<JobPosting> _jobs;

    public JobManager()
    {
        _jobs = _storage.LoadJobs();
    }

    public List<JobPosting> GetAllJobs()
    { 
        return _jobs;
    }

    public void AddJob(JobPosting job)
    {
        job.Id = _jobs.Count == 0 ? 1 : _jobs.Max(j => j.Id) + 1;
        _jobs.Add(job);
        _storage.SaveJobs(_jobs);
    }

    public JobPosting? GetJobById(int id)
    {
        return _jobs.FirstOrDefault(j => j.Id == id);
    }

    public bool DeleteJob(int id)
    {
        JobPosting? job = GetJobById(id);

        if (job == null)
        {
            return false;
        }

        _jobs.Remove(job);
        _storage.SaveJobs(_jobs);
        return true;
    }
}