namespace Otb.NodeSequencer.Service
{
    public class NodeWrapper<T> where T : INode
    {
        private enum VisitState
        {
            None = 0,
            Permanent = 1,
            Temporary = 2
        }

        private VisitState _mark;

        public T Node { get; }

        public NodeWrapper(T node)
        {
            Node = node;
            _mark = VisitState.None;
        }

        public bool IsPermanent => _mark == VisitState.Permanent;

        public bool IsTemporary => _mark == VisitState.Temporary;

        public void SetPermanent() => _mark = VisitState.Permanent;

        public void SetTemporary() => _mark = VisitState.Temporary;
    }
}
