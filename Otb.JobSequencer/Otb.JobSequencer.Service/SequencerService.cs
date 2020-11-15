using Otb.JobSequencer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Otb.JobSequencer.Service
{
    public class SequencerService : ISequencerService
    {
        private IList<JobWrapper> _jobWrappers;
        private Stack<IJob> _sequencedJobs;

        private void Visit(JobWrapper jobWrapper)
        {
            if (jobWrapper.IsPermanent) return;
            if (jobWrapper.IsTemporary) throw new ArgumentException("Jobs can not have circular dependencies");

            jobWrapper.SetTemporary();

            foreach (var referencingJobWrapper in GetJobsDependentOnJob(jobWrapper.Job))
            {
                Visit(referencingJobWrapper);
            }

            jobWrapper.SetPermanent();
            _sequencedJobs.Push(jobWrapper.Job);
        }

        private IEnumerable<JobWrapper> GetJobsDependentOnJob(IJob job)
        {
            var reliantJobWrappers = _jobWrappers.Where(x => x.Job.Dependency == job.Name);
            return reliantJobWrappers;
        }

        private static List<JobWrapper> GetJobWrappers(IEnumerable<IJob> jobs) =>
            jobs.Select(x => new JobWrapper(x)).ToList();

        private JobWrapper GetUnmarkedJobWrapper() =>
            _jobWrappers.First(x => !x.IsPermanent);

        private bool JobsExistWithoutPermanentMark() =>
            _jobWrappers.Count(x => !x.IsPermanent) > 0;

        /// <summary>
        /// Gets the topological ordering.
        /// </summary>
        /// <param name="jobs">The jobs.</param>
        /// <remarks>
        /// This uses the Depth-First search method
        /// Source: https://en.wikipedia.org/wiki/Topological_sorting
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<IJob> GetTopologicalOrdering(IEnumerable<IJob> jobs)
        {
            _jobWrappers = GetJobWrappers(jobs);

            _sequencedJobs = new Stack<IJob>();

            while (JobsExistWithoutPermanentMark())
            {
                var unmarkedJobWrapper = GetUnmarkedJobWrapper();
                Visit(unmarkedJobWrapper);
            }

            var result = _sequencedJobs.ToList();

            return result;
        }
    }
}
