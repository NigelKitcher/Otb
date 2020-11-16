using System;
using System.Collections.Generic;
using System.Linq;

namespace Otb.NodeSequencer.Service
{
    /// <summary>
    /// Generic implementation of Depth-First method of ordering nodes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Otb.NodeSequencer.Service.INodeSequencerService{T}" />
    /// <remarks>
    /// Source: https://en.wikipedia.org/wiki/Topological_sorting
    /// </remarks>
    public class NodeSequencerService<T> : INodeSequencerService<T> 
        where T : INode
    {
        private IList<NodeWrapper<T>> _nodeWrappers;
        private Stack<T> _sequencedNodes;

        private void Visit(NodeWrapper<T> nodeWrapper)
        {
            var typeNamePlural = typeof(T).Name + "s";

            if (nodeWrapper.IsPermanent) return;
            if (nodeWrapper.IsTemporary) throw new ArgumentException($"{typeNamePlural} can not have circular dependencies");

            nodeWrapper.SetTemporary();

            foreach (var referencingNodeWrapper in GetNodesDependentOnNode(nodeWrapper.Node))
            {
                Visit(referencingNodeWrapper);
            }

            nodeWrapper.SetPermanent();
            _sequencedNodes.Push(nodeWrapper.Node);
        }

        private IEnumerable<NodeWrapper<T>> GetNodesDependentOnNode(T node)
        {
            var reliantNodeWrappers = _nodeWrappers.Where(x => x.Node.Dependency == node.Name);
            return reliantNodeWrappers;
        }

        private static List<NodeWrapper<T>> GetNodeWrappers(IEnumerable<T> nodes) =>
            nodes.Select(x => new NodeWrapper<T>(x)).ToList();

        private NodeWrapper<T> GetUnmarkedNodeWrapper() =>
            _nodeWrappers.First(x => !x.IsPermanent);

        private bool NodesExistWithoutPermanentMark() =>
            _nodeWrappers.Any(x => !x.IsPermanent);

        public IEnumerable<T> GetTopologicalOrdering(IEnumerable<T> nodes)
        {
            _nodeWrappers = GetNodeWrappers(nodes);

            _sequencedNodes = new Stack<T>();

            while (NodesExistWithoutPermanentMark())
            {
                var unmarkedNodeWrapper = GetUnmarkedNodeWrapper();
                Visit(unmarkedNodeWrapper);
            }

            var result = _sequencedNodes.ToList<T>();

            return result;
        }
    }
}
