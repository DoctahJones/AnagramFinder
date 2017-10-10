using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagrams
{
    public class AnagramFinder
    {
        private Dictionary<char, int> characterPrimeValues;

        private List<string> longStrings;

        public IPrimeGenerator PrimeGenerator { get; set; }

        public AnagramFinder(bool loadDefaults = true)
        {
            characterPrimeValues = new Dictionary<char, int>();

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
        public virtual List<List<string>> FindAnagrams(IEnumerable<string> inputList)
        {
            longStrings = new List<string>();
            Dictionary<UInt64, List<string>> encodedStrings = new Dictionary<UInt64, List<string>>();
            foreach (string currString in inputList)
            {
                UInt64 encodedStringValue = EncodeStringWithPrimes(currString);
                if (encodedStringValue != 1)
                {
                    List<string> strings;
                    if (encodedStrings.TryGetValue(encodedStringValue, out strings))
                    {
                        strings.Add(currString);
                    }
                    else
                    {
                        encodedStrings.Add(encodedStringValue, new List<string>() { currString });
                    }
                }
            }
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
                var results = CheckLongerStringsForAnagrams(longStrings);
                outputList = outputList.Union(results).ToList();
            }
            return outputList;
        }

        /// <summary>
        /// Uses an actually good method to look for anagrams.
        /// </summary>
        /// <param name="longStrings">The list of strings to </param>
        /// <returns>Returns list of anagrams found.</returns>
        protected List<List<string>> CheckLongerStringsForAnagrams(List<string> longStrings)
        {
            Dictionary<string, List<string>> anagramCollection = new Dictionary<string, List<string>>();

            foreach (string currString in longStrings)
            {
                string currStringSorted = String.Concat(currString.OrderBy(c => c)).ToLower();
                List<String> currAnagramList;
                if (anagramCollection.TryGetValue(currStringSorted, out currAnagramList))
                {
                    currAnagramList.Add(currString);
                }
                else
                {
                    anagramCollection.Add(currStringSorted, new List<string>() { currString });
                }
            }
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
                    int prime = PrimeGenerator.GetNextPrime();
                    characterPrimeValues.Add(c, prime);
                    charValue = prime;
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
            characterPrimeValues.Add('e', PrimeGenerator.CurrentPrime);
            characterPrimeValues.Add('t', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('a', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('o', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('i', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('n', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('s', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('h', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('r', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('d', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('l', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('c', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('u', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('m', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('w', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('f', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('g', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('y', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('p', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('b', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('v', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('k', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('j', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('x', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('q', PrimeGenerator.GetNextPrime());
            characterPrimeValues.Add('z', PrimeGenerator.GetNextPrime());
        }

    }
}
