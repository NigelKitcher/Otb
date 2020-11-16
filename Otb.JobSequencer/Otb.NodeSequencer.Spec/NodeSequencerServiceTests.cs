using Microsoft.VisualStudio.TestTools.UnitTesting;
using Otb.NodeSequencer.Service;
using Otb.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Otb.NodeSequencer.Spec
{
    [TestClass]
    public class NodeSequencerServiceTests
    {
        private INodeSequencerService<FakeNode> _sequencerService;

        [TestInitialize]
        public void Initialise()
        {
            _sequencerService = new NodeSequencerService<FakeNode>();
        }

        [TestMethod]
        public void Given_no_nodes_When_GetTopologicalOrdering_is_invoked_Then_returns_no_node()
        {
            // Arrange

            // Act
            var result = _sequencerService.GetTopologicalOrdering(new List<FakeNode>()).ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Given_a_node_When_GetTopologicalOrdering_is_invoked_Then_returns_one_node()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeNode("A")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("A", result[0].Name);
        }

        [TestMethod]
        public void Given_three_nodes_When_GetTopologicalOrdering_is_invoked_Then_returns_the_three_nodes_in_no_specific_order()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeNode("A"),
                new FakeNode("B"),
                new FakeNode("C")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsNotNull(result.Single(x => x.Name == "A"));
            Assert.IsNotNull(result.Single(x => x.Name == "B"));
            Assert.IsNotNull(result.Single(x => x.Name == "C"));
        }

        [TestMethod]
        public void Given_three_nodes_where_one_has_a_dependency_and_one_does_not_When_GetTopologicalOrdering_is_invoked_Then_returns_the_three_nodes_and_dependency_in_correct_order()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeNode("A"),
                new FakeLinkedNode("B", "C"),
                new FakeNode("C")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsNotNull(result.Single(x => x.Name == "A"));
            Assert.That.IsOrdered(new Tuple<string, string>("C", "B"), result);
        }

        [TestMethod]
        public void Given_nodes_with_multiple_references_as_per_fourth_test_case_from_requirements()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeNode("A"),
                new FakeLinkedNode("B", "C"),
                new FakeLinkedNode("C", "F"),
                new FakeLinkedNode("D", "A"),
                new FakeLinkedNode("E", "B"),
                new FakeNode("F")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
            Assert.AreEqual(6, result.Count);
            Assert.IsNotNull(result.Single(x => x.Name == "A"));
            Assert.IsNotNull(result.Single(x => x.Name == "B"));
            Assert.IsNotNull(result.Single(x => x.Name == "C"));
            Assert.IsNotNull(result.Single(x => x.Name == "D"));
            Assert.IsNotNull(result.Single(x => x.Name == "E"));
            Assert.IsNotNull(result.Single(x => x.Name == "F"));
            Assert.That.IsOrdered(new Tuple<string, string>("F", "C"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("C", "B"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("B", "E"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("A", "D"), result);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeNodes can not have circular dependencies")]
        public void Given_three_nodes_with_a_self_dependency_When_GetTopologicalOrdering_is_invoked_Then_exception_thrown()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeNode("A"),
                new FakeNode("B"),
                new FakeLinkedNode("C", "C")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeNodes can not have circular dependencies")]
        public void Given_a_list_of_nodes_that_have_circular_dependency_as_per_sixth_case_in_requirements_When_GetTopologicalOrdering_is_invoked_Then_Exception_thrown()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeNode("A"),
                new FakeLinkedNode("B", "C"),
                new FakeLinkedNode("C", "F"),
                new FakeLinkedNode("D", "A"),
                new FakeNode("E"),
                new FakeLinkedNode("F", "B")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
        }

        [TestMethod]
        public void Given_a_node_with_a_backwards_dependency_When_GetTopologicalOrdering_is_invoked_Then_two_nodes_GetTopologicalOrdering_is_invoked_with_correct_order()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeNode("A"),
                new FakeLinkedNode("B", "A"),
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.That.IsOrdered(new Tuple<string, string>("A", "B"), result);
        }

        [TestMethod]
        public void Given_a_node_with_a_forwards_dependency_When_GetTopologicalOrdering_is_invoked_Then_two_nodes_GetTopologicalOrdering_is_invoked_with_correct_order()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeLinkedNode("A", "B"),
                new FakeNode("B"),
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.That.IsOrdered(new Tuple<string, string>("B", "A"), result);
        }

        [TestMethod]
        public void Given_a_node_with_a_forwards_dependency_to_a_node_that_has_a_backwards_dependency_When_GetTopologicalOrdering_is_invoked_Then_three_nodes_GetTopologicalOrdering_is_invoked_with_correct_order()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeLinkedNode("A", "C"),
                new FakeNode("B"),
                new FakeLinkedNode("C", "B")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.That.IsOrdered(new Tuple<string, string>("B", "C"), result);
            Assert.That.IsOrdered(new Tuple<string, string>("C", "A"), result);
        }

        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeNodes can not have circular dependencies")]
        public void Given_a_node_with_a_self_dependency_When_GetTopologicalOrdering_is_invoked_Then_exception_thrown()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeLinkedNode("A", "A")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
        }


        [TestMethod, ExpectedExceptionWithMessage(typeof(ArgumentException), "FakeNodes can not have circular dependencies")]
        public void Given_two_nodes_with_a_circular_dependency_When_GetTopologicalOrdering_is_invoked_Then_exception_thrown()
        {
            // Arrange
            var nodes = new List<FakeNode>
            {
                new FakeLinkedNode("A", "B"),
                new FakeLinkedNode("B", "A")
            };

            // Act
            var result = _sequencerService.GetTopologicalOrdering(nodes).ToList();

            // Assert
        }
    }
}
