namespace Otb.JobSequencer.Contracts
{
    public interface ISequencerService
    {
        /// <summary>
        /// Gets the topological ordering.
        /// </summary>
        /// <param name="jobRequest">The job request which contains the jobs names and dependencies formatted as per spec</param>
        /// <returns>A string of the job names in order</returns>
        string GetTopologicalOrdering(string jobRequest);
    }
}
