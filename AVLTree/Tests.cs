using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AVLTree
{
    [TestFixture]
    class Tests
    {
        AVLTree<int> avlTree = new AVLTree<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 19, 18, 17, 16, 15, 14, 13, 12, 11 };
        [SetUp]
        public void SetItUp()
        {

        }
        [Test]
        public void RemoveTest()
        {
            AVLTree<int> actual = new AVLTree<int>
           {
               10,5,11,6,12
           };
            actual.Remove(6);
            AVLTree<int> expected = new AVLTree<int>
           {
               10,5,11,12
           };
            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void AddTest()
        {
            AVLTree<int> actual = new AVLTree<int> { 2, 5, 6, 9 };
            actual.Add(8);
            AVLTree<int> expected = new AVLTree<int> { 2, 5, 6, 9, 8 };
            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void SumTest()
        {
            AVLTree<int> actual = new AVLTree<int> { 2, 5, 6, 9 };
            int expected = 22;
            Assert.AreEqual(expected, actual.Sum());
        }
        [Test]
        public void ContainsTest()
        {
            AVLTree<int> actual = new AVLTree<int> { 2, 5, 6, 9 };
            //bool expected = true;
            Assert.IsTrue(actual.Contains(6));
        }
        [Test]
        public void FindNodeTest()
        {
            int expected = 8;
            Assert.AreEqual(expected, avlTree.FindNode(8).Value);
        }
        [Test]
        public void BalanceTest()
        {
            AVLTree<int> actual = new AVLTree<int>();
            actual.Add(1);
            actual.Add(2);
            actual.Add(3);
            actual.Add(4);
            actual.Add(5);
            actual.Add(6);
            actual.Remove(4);
            AVLTree<int> expected = new AVLTree<int> { 1,2,3,5,6};
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
