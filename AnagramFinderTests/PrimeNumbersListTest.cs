using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Anagrams;

namespace AnagramFinderTests
{
    [TestClass]
    public class PrimeNumbersListTest
    {
        [TestMethod]
        public void TestGetFirstFivePrimesFromIndex()
        {
            int[] expected = new int[] { 2, 3, 5, 7, 11 };

            int[] actual = new int[5];
            PrimeNumbersList prime = new PrimeNumbersList();
            actual[0] = prime.GetNthPrime(1);
            actual[1] = prime.GetNthPrime(2);
            actual[2] = prime.GetNthPrime(3);
            actual[3] = prime.GetNthPrime(4);
            actual[4] = prime.GetNthPrime(5);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetFirstFivePrimesFromOrdered()
        {
            int[] expected = new int[] { 2, 3, 5, 7, 11 };

            int[] actual = new int[5];
            PrimeNumbersList prime = new PrimeNumbersList();
            actual[0] = prime.CurrentPrime;
            actual[1] = prime.GetNextPrime();
            actual[2] = prime.GetNextPrime();
            actual[3] = prime.GetNextPrime();
            actual[4] = prime.GetNextPrime();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSetCurrentIndexHigherThanCurrent()
        {
            PrimeNumbersList prime = new PrimeNumbersList();
            Assert.AreEqual(1, prime.CurrentPrimeIndex);

            prime.CurrentPrimeIndex = 4;
            Assert.AreEqual(4, prime.CurrentPrimeIndex);

            prime.CurrentPrimeIndex = 200;
            Assert.AreEqual(200, prime.CurrentPrimeIndex);
        }

        [TestMethod]
        public void TestSetCurrentIndexSameAsCurrent()
        {
            PrimeNumbersList prime = new PrimeNumbersList();
            Assert.AreEqual(1, prime.CurrentPrimeIndex);

            prime.CurrentPrimeIndex = 4;
            Assert.AreEqual(4, prime.CurrentPrimeIndex);

            prime.CurrentPrimeIndex = 4;
            Assert.AreEqual(4, prime.CurrentPrimeIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetCurrentIndexLowerThanCurrent()
        {
            PrimeNumbersList prime = new PrimeNumbersList();
            Assert.AreEqual(1, prime.CurrentPrimeIndex);

            prime.CurrentPrimeIndex = 4;
            Assert.AreEqual(4, prime.CurrentPrimeIndex);

            prime.CurrentPrimeIndex = 3;
            Assert.AreEqual(3, prime.CurrentPrimeIndex);
        }

        [TestMethod]
        public void TestGetCurrentPrime()
        {
            PrimeNumbersList prime = new PrimeNumbersList();
            
            prime.CurrentPrimeIndex = 4;
            Assert.AreEqual(7, prime.CurrentPrime);

            prime.CurrentPrimeIndex = 371;
            Assert.AreEqual(2539, prime.CurrentPrime);
        }

        [TestMethod]
        public void TestGetLastFivePrimesFromIndex()
        {
            int[] expected = new int[] { 7879, 7883, 7901, 7907, 7919 };

            int[] actual = new int[5];
            PrimeNumbersList prime = new PrimeNumbersList();

            int count = prime.MaxPrimeIndex;

            actual[0] = prime.GetNthPrime(count - 4);
            actual[1] = prime.GetNthPrime(count - 3);
            actual[2] = prime.GetNthPrime(count - 2);
            actual[3] = prime.GetNthPrime(count - 1);
            actual[4] = prime.GetNthPrime(count - 0);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllPrimesInListInOrder()
        {
            int[] expected = new int[] { 7879, 7883, 7901, 7907, 7919 };

            PrimeNumbersList prime = new PrimeNumbersList();

            int current = prime.CurrentPrime;
            for (int i = 0; i < prime.MaxPrimeIndex - 1; i++)
            {
                current = prime.GetNextPrime();
            }
            Assert.AreEqual(7919, current);


        }

    }
}
