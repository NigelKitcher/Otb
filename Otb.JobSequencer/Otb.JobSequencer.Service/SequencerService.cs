using Otb.JobSequencer.Contracts;
using System;
using System.Collections.Generic;

namespace Otb.JobSequencer.Service
{
    public class SequencerService : ISequencerService
    {
        public IEnumerable<IJob> GetTopologicalOrdering(IEnumerable<IJob> nodes)
        {
            throw new NotImplementedException();
        }
    }
}
