using Otb.JobSequencer.Contracts;
using Otb.NodeSequencer.Service;
using System.Text;

namespace Otb.JobSequencer.Service
{
    public class SequencerService : ISequencerService
    {
        private readonly JobRequestParser _jobRequestParser = new JobRequestParser();
        private readonly INodeSequencerService<Job> _sequencerService = new NodeSequencerService<Job>(); //TODO: may want to inject this so can swap out different implementations

        public string GetTopologicalOrdering(string jobRequest)
        {
            var jobs = _jobRequestParser.GetJobs(jobRequest);
            var sequencedJobs = _sequencerService.GetTopologicalOrdering(jobs);

            var result = new StringBuilder();
            foreach (var job in sequencedJobs)
            {
                result.Append(job.Name);
            }

            return result.ToString();
        }
    }
}
