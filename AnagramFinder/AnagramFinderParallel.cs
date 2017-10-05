using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagrams
{
    public class AnagramFinderParallel : AnagramFinder
    {
        private ConcurrentDictionary<char, int> characterPrimeValues;
        private Object primeValuesAddLock = new Object();

        private ConcurrentDictionary<int, string> encodedStrings;
        private Dictionary<int, List<string>> anagramCollection;
        private Object anagramLock = new Object();

        public AnagramFinderParallel(bool loadDefaults = false)
        {
            //TODO defaults
            PrimeGenerator = new PrimeNumbersList();

            characterPrimeValues = new ConcurrentDictionary<char,int>();
        }


        public override List<List<string>> FindAnagrams(IEnumerable<string> inputList)
        {
            encodedStrings = new ConcurrentDictionary<int, string>();
            anagramCollection = new Dictionary<int, List<string>>();

            Parallel.ForEach(inputList, EncodeStringAndAddIfAnagram);

            var outputList = new List<List<string>>();
            foreach (var encodedValue in anagramCollection)
            {
                if (encodedValue.Value.Count() > 1)
                {
                    outputList.Add(encodedValue.Value);
                }
            }
            return outputList;
        }

        private void EncodeStringAndAddIfAnagram(string currString)
        {
            int encodedStringValue = EncodeStringWithPrimes(currString);
            if (encodedStringValue != 1)
            {
                string oneStringWithThisValue;
                if (encodedStrings.TryGetValue(encodedStringValue, out oneStringWithThisValue))
                {
                    lock (anagramLock)
                    {
                        List<string> listAnagram;
                        if (anagramCollection.TryGetValue(encodedStringValue, out listAnagram))
                        {
                            listAnagram.Add(currString);
                        }
                        else
                        {
                            anagramCollection.Add(encodedStringValue, new List<string>() { oneStringWithThisValue, currString });
                        }
                    }
                }
                else
                {
                    if (!encodedStrings.TryAdd(encodedStringValue, currString))
                    {
                        lock (anagramLock)
                        {
                            anagramCollection[encodedStringValue].Add(currString);
                        }
                    }
                }
            }
        }



        private int EncodeStringWithPrimes(string currString)
        {
            int encodedStringValue = 1;
            foreach (char c in currString)
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
                encodedStringValue *= charValue;
            }
            return encodedStringValue;
        }
    }
}

