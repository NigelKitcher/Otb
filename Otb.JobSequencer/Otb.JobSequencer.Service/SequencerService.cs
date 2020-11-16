using Otb.JobSequencer.Contracts;
using Otb.NodeSequencer.Service;
using System.Text;

namespace Otb.JobSequencer.Service
{
    public class SequencerService : ISequencerService
    {
        public string GetTopologicalOrdering(string jobRequest)
        {
            var jobRequestParser = new JobRequestParser();
            var sequencerService = new NodeSequencerService<Job>();

            var jobs = jobRequestParser.GetJobs(jobRequest);

            var sequencedJobs = sequencerService.GetTopologicalOrdering(jobs);

            var result = new StringBuilder();
            foreach (var job in sequencedJobs)
            {
                result.Append(job.Name);
            }

            return result.ToString();
        }
    }
}
