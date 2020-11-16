using System.Collections.Generic;

namespace Otb.NodeSequencer.Service
{
    public interface INodeSequencerService<T> where T : INode
    {
        IEnumerable<T> GetTopologicalOrdering(IEnumerable<T> nodes);
    }
}
