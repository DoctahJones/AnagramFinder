using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anagrams
{
    public class AnagramFinderParallel : AnagramFinder
    {
        private ConcurrentDictionary<char, int> characterPrimeValues;
        private Object primeValuesAddLock = new Object();

        private ConcurrentDictionary<UInt64, List<string>> encodedStrings;
        private Object anagramLock = new Object();

        private ConcurrentBag<string> longStrings;


        public AnagramFinderParallel(bool loadDefaults = true)
        {
            characterPrimeValues = new ConcurrentDictionary<char, int>();
            PrimeGenerator = new PrimeNumbersList();
            if (loadDefaults)
            {
                LoadSimpleDefaults();
            }
        }

        /// <summary>
        /// Finds the anagrams in a collection of words.
        /// </summary>
        /// <param name="inputList">Strings to search within.</param>
        /// <returns>List of Lists with each being a group of words that are anagrams of one another.</returns>
        public override List<List<string>> FindAnagrams(IEnumerable<string> inputList)
        {
            encodedStrings = new ConcurrentDictionary<UInt64, List<string>>();
            longStrings = new ConcurrentBag<string>();

            Parallel.ForEach(inputList, EncodeStringAndAddToDictionary);

            var outputList = new List<List<string>>();
            foreach (var encodedValue in encodedStrings)
            {
                if (encodedValue.Value.Count() > 1)
                {
                    outputList.Add(encodedValue.Value);
                }
            }

            if (longStrings.Count > 0)
            {
                var results = CheckLongerStringsForAnagrams(longStrings.ToList());
                outputList = outputList.Union(results).ToList();
            }
            return outputList;
        }

        /// <summary>
        /// Encodes individual string with primes and adds it to the dictinary encodedStrings either on its own or with any previous that encoded to same value.
        /// </summary>
        /// <param name="currString">Current string to encode.</param>
        private void EncodeStringAndAddToDictionary(string currString)
        {
            UInt64 encodedStringValue = EncodeStringWithPrimes(currString);
            if (encodedStringValue != 1)
            {
                List<string> anagramList;
                if (encodedStrings.TryGetValue(encodedStringValue, out anagramList))
                {
                    lock (anagramLock)
                    {
                        //reget list in case something was changed before getting lock.
                        encodedStrings.TryGetValue(encodedStringValue, out anagramList);
                        anagramList.Add(currString);
                    }
                }
                else
                {
                    if (!encodedStrings.TryAdd(encodedStringValue, new List<string>() { currString }))
                    {
                        lock (anagramLock)
                        {
                            //reget list in case something was changed before getting lock.
                            encodedStrings.TryGetValue(encodedStringValue, out anagramList);
                            anagramList.Add(currString);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Encodes string by giving each character a prime number and calculating the value for the string as a product of primes.
        /// </summary>
        /// <param name="currString">The String to encode.</param>
        /// <returns>An in representing the string as a product of primes. Returns 1 if int overflow on long word etc.</returns>
        private UInt64 EncodeStringWithPrimes(string currString)
        {
            UInt64 encodedStringValue = 1;
            foreach (char c in currString.ToLower())
            {
                int charValue;
                if (!characterPrimeValues.TryGetValue(c, out charValue))
                {
                    lock (primeValuesAddLock)
                    {
                        int prime = PrimeGenerator.GetNextPrime();
                        //check that we add value successfully. if some thread entered lock after the charvalue was added by another thread then this will fail.
                        if (characterPrimeValues.TryAdd(c, prime))
                        {
                            charValue = prime;
                        }
                        else
                        {
                            characterPrimeValues.TryGetValue(c, out charValue);
                        }
                    }
                }
                try
                {
                    encodedStringValue = checked(encodedStringValue * (UInt64)charValue);
                }
                catch (OverflowException)
                {
                    longStrings.Add(currString);
                    encodedStringValue = 1;
                    break;
                }
            }
            return encodedStringValue;
        }

        /// <summary>
        /// Loads default lower case letters using frequency of use as method to use lower prime number.
        /// </summary>
        private void LoadSimpleDefaults()
        {
            characterPrimeValues.TryAdd('e', PrimeGenerator.CurrentPrime);
            characterPrimeValues.TryAdd('t', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('a', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('o', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('i', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('n', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('s', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('h', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('r', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('d', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('l', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('c', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('u', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('m', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('w', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('f', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('g', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('y', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('p', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('b', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('v', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('k', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('j', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('x', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('q', PrimeGenerator.GetNextPrime());
            characterPrimeValues.TryAdd('z', PrimeGenerator.GetNextPrime());
        }

    }
}

