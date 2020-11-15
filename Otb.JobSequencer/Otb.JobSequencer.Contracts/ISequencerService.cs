using System.Collections.Generic;

namespace Otb.JobSequencer.Contracts
{
    public interface ISequencerService
    {
        IEnumerable<IJob> GetTopologicalOrdering(IEnumerable<IJob> nodes);
    }
}
