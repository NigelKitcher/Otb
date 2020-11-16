using Otb.JobSequencer.Service;
using Otb.NodeSequencer.Service;

namespace Otb.NodeSequencer.Spec
{
    public class FakeJob : Job, INode
    {
        public FakeJob(string name, string dependency) : base(name, dependency)
        {
        }

        public FakeJob(string name) : base(name)
        {
        }
    }
}
