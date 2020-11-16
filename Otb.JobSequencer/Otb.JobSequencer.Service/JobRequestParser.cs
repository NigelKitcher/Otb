using System;
using System.Collections.Generic;
using System.Linq;

namespace Otb.JobSequencer.Service
{
    public class JobRequestParser
    {
        private const int JobNameStartPosition = 0;
        private const int DependencyNameStartPosition = 5;
        private const string DependsOnDesignator = "=>";
        private const int ValidJobCharacterCount = 4;
        private const int ValidJobWithDependencyCharacterCount = 6;

        private static string GetName(string line, int startPosition)
        {
            var name = line.Substring(startPosition, 1);
            if (!char.IsLetter(name[0])) throw new ArgumentException("Job name is not an alpha character");
            return name;
        }

        private string GetJobName(string line)
        {
            return GetName(line, JobNameStartPosition);
        }

        private string GetDependency(string line)
        {
            return GetName(line, DependencyNameStartPosition);
        }

        private static void CheckSpaces(string line)
        {
            if (IsMissingFirstSpace(line)) throw new ArgumentException("Missing space");
            if (IsPotentialForDependency(line) && IsMissingSecondSpace(line)) throw new Exception("Missing space");
        }

        private static bool IsMissingFirstSpace(string line)
        {
            return line.Substring(1, 1) != " ";
        }

        private static bool IsPotentialForDependency(string line)
        {
            return line.Length == ValidJobWithDependencyCharacterCount;
        }

        private static bool IsMissingSecondSpace(string line)
        {
            return line.Substring(4, 1) != " ";
        }

        private static void ValidateLineFormat(string line)
        {
            if (line.Length != ValidJobCharacterCount && line.Length != ValidJobWithDependencyCharacterCount) throw new ArgumentException("Invalid line length");
            CheckSpaces(line);

            var pointer = line.Substring(2, DependsOnDesignator.Length);
            if (pointer != DependsOnDesignator) throw new ArgumentException("Missing pointer");

            if (line.Length == ValidJobWithDependencyCharacterCount && line.Substring(4, 1) != " ") throw new Exception("Missing space");
        }

        private static void CheckDependentsExist(List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                if (!job.HasDependency) continue;

                if (jobs.All(x => x.Name != job.Dependency)) throw new ArgumentException($"Missing definition for job {job.Dependency}");
            }

        }

        public IEnumerable<Job> GetJobs(string jobRequest)
        {
            if (jobRequest == null) throw new ArgumentNullException(nameof(jobRequest));

            var jobs = new List<Job>();
            var lines = jobRequest.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (line.Length == 0) continue;
                ValidateLineFormat(line);
                var name = GetJobName(line);

                if (line.Length == ValidJobCharacterCount)
                {
                    jobs.Add(new Job(name));
                }
                else
                {
                    var dependency = GetDependency(line);
                    if (dependency == name) throw new ArgumentException("Jobs can not depend on themselves");
                    jobs.Add(new Job(name, dependency));
                }
            }

            CheckDependentsExist(jobs);

            return jobs;
        }
    }
}
