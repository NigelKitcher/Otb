using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Otb.JobSequencer.Contracts;

namespace Otb.TestTools.UnitTesting
{
    public static class AssertExtensions
    {
        /// <summary>
        /// Determines whether the two names of nodes appear in correct order in the enumerable collection.
        /// </summary>
        /// <param name="_">The assert.</param>
        /// <param name="expected">A tuple of two values denoting the order they show appear</param>
        /// <param name="actual">The actual collection of nodes</param>
        public static void IsOrdered(this Assert _, Tuple<string, string> expected, IEnumerable<IJob> actual)
        {
            var actualOrder = actual.ToList();
            var dependency = expected.Item1;
            var reliant = expected.Item2;

            var indexOfDependency = actualOrder.IndexOf(actualOrder.Single(x => x.Name == dependency));
            var indexOfReliant = actualOrder.IndexOf(actualOrder.Single(x => x.Name == reliant));

            Assert.IsTrue(indexOfDependency < indexOfReliant, "Dependency object does not appear before the object that relies on it");
        }
    }
}
