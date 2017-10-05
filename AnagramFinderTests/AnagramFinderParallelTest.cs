using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Anagrams;
using System.Collections.Generic;

namespace AnagramFinderParallelTests
{
    [TestClass]
    public class AnagramFinderParallelParallelTest
    {
        [TestMethod]
        public void TestAnagramFinderParallelFindsNoAnagrams()
        {
            AnagramFinderParallel af = new AnagramFinderParallel();

            var wordList = new List<string>() { "act", "dog", "fish", "ac", "ca t", "mouse" };

            var anagrams = af.FindAnagrams(wordList);

            Assert.AreEqual(0, anagrams.Count);
        }


        [TestMethod]
        public void TestAnagramFinderParallelFindsPair()
        {
            AnagramFinderParallel af = new AnagramFinderParallel();

            var wordList = new List<string>() { "act", "cat" };

            var anagrams = af.FindAnagrams(wordList);

            Assert.AreEqual(1, anagrams.Count);
            CollectionAssert.AreEquivalent(wordList, anagrams.ToArray()[0], "Outputted: " + String.Join(", ", anagrams.ToArray()[0]));
        }

        [TestMethod]
        public void TestAnagramFinderParallelFindsPairNotOther()
        {
            AnagramFinderParallel af = new AnagramFinderParallel();

            var wordList = new List<string>() { "act", "cat", "dog", "fish", "ac", "ca t" };

            var expected = new List<string>() { "act", "cat" };

            var anagrams = af.FindAnagrams(wordList);

            Assert.AreEqual(1, anagrams.Count);
            CollectionAssert.AreEquivalent(expected, anagrams.ToArray()[0]);
        }

        [TestMethod]
        public void TestAnagramFinderParallelFindsMoreThanPair()
        {
            AnagramFinderParallel af = new AnagramFinderParallel();

            var wordList = new List<string>() { "apers", "apres", "asper", "pares", "parse", "pears", "prase", "presa", "rapes", "reaps", "spare", "spear" };

            var anagrams = af.FindAnagrams(wordList);

            Assert.AreEqual(1, anagrams.Count);
            CollectionAssert.AreEquivalent(wordList, anagrams.ToArray()[0]);
        }

        [TestMethod]
        public void TestAnagramFinderParallelFindsMultipleSets()
        {
            AnagramFinderParallel af = new AnagramFinderParallel();

            var cat = new List<string>() { "cat", "act" };
            var dog = new List<string>() { "dog", "god" };
            var wordList = cat.Union(dog);

            var anagrams = af.FindAnagrams(wordList);

            Assert.AreEqual(2, anagrams.Count);

            bool catFound = false;
            bool dogFound = false;

            var first = anagrams.ElementAt(0);
            Assert.AreEqual(2, first.Count());
            var second = anagrams.ElementAt(1);
            Assert.AreEqual(2, second.Count());
            if (first.Except(cat).Count() == 0)
            {
                catFound = !catFound;
            }
            if (first.Except(dog).Count() == 0)
            {
                dogFound = !dogFound;
            }
            if (second.Except(cat).Count() == 0)
            {
                catFound = !catFound;
            }
            if (second.Except(dog).Count() == 0)
            {
                dogFound = !dogFound;
            }
            Assert.IsTrue(catFound && dogFound);
        }

        [TestMethod]
        public void TestAnagramFinderParallelFindsLongAnagrams()
        {
            AnagramFinderParallel af = new AnagramFinderParallel();
            var an = new List<string>() { "hydroxydeoxycorticosterones", "hydroxydesoxycorticosterone" };
            var extra = new List<string>() { "doggygotoshoptoday", "fishinthewatergoswimswimswim", "Thanks for getting in touch, we are currently experiencing high contact volumes, we apologise for any delays in responding to your email and appreciate your patience. We will respond to you as quickly as possible." };
            var wordList = an.Union(extra);

            var anagrams = af.FindAnagrams(wordList);

            Assert.AreEqual(1, anagrams.Count);
            CollectionAssert.AreEquivalent(an, anagrams.ToArray()[0]);

        }
    }
}
