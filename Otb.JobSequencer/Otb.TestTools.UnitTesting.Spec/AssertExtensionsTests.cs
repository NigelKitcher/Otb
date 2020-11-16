using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Otb.TestTools.UnitTesting.Spec
{
    [TestClass]
    public class AssertExtensionsTests
    {
        [TestMethod]
        public void Given_jobs_in_correct_order__When_IsOrdered_invoked_Then_test_passes()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B")
            };

            // Act
            Assert.That.IsOrdered(new Tuple<string, string>("A", "B"), jobs);

            // Assert
        }

        [TestMethod, ExpectedException(typeof(AssertFailedException))]
        public void Given_jobs_not_in_correct_order_When_IsOrdered_invoked_Then_test_fails_and_throws_AssertFailedException()
        {
            // Arrange
            var jobs = new List<FakeJob>
            {
                new FakeJob("A"),
                new FakeJob("B")
            };

            // Act
            Assert.That.IsOrdered(new Tuple<string, string>("B", "A"), jobs);

            // Assert
        }
    }
}
