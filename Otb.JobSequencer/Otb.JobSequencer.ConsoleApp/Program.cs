using System;
using Otb.JobSequencer.Service;

namespace Otb.JobSequencer.ConsoleApp
{
    class Program
    {
        static void Main(string[] _)
        {
            var sequencerService = new SequencerService();

            var input = "A => B" + Environment.NewLine + "B =>";
            Console.WriteLine("Input Jobs:");
            Console.WriteLine(input);

            Console.WriteLine("Sequenced Jobs:");
            var output = sequencerService.GetTopologicalOrdering(input);
            Console.WriteLine(output);

            Console.WriteLine("Completed.");
        }
    }
}
