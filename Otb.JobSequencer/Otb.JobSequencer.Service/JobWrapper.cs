using Otb.JobSequencer.Contracts;

namespace Otb.JobSequencer.Service
{
    internal class JobWrapper
    {
        private enum VisitState
        {
            None = 0,
            Permanent = 1,
            Temporary = 2
        }

        private VisitState _mark;

        public IJob Job { get; }

        public JobWrapper(IJob job)
        {
            Job = job;
            _mark = VisitState.None;
        }

        public bool IsPermanent => _mark == VisitState.Permanent;

        public bool IsTemporary => _mark == VisitState.Temporary;

        public void SetPermanent() => _mark = VisitState.Permanent;

        public void SetTemporary() => _mark = VisitState.Temporary;
    }
}
