namespace Otb.JobSequencer.Contracts
{
    public interface IJob
    {
        string Name { get; }
        string Dependency { get; }
    }
}
