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

        public IPrimeGenerator PrimeGenerator { get; set; }

        public AnagramFinder(bool loadDefaults = false)
        {
            if (loadDefaults)
            {
                characterPrimeValues = new Dictionary<char, int>(GetSimpleDefaults());
            }
            else
            {
                characterPrimeValues = new Dictionary<char, int>();
            }
            PrimeGenerator = new PrimeNumbersList();
        }

        protected IDictionary<char, int> GetSimpleDefaults()
        {
            throw new NotImplementedException();
        }

        public virtual List<List<string>> FindAnagrams(IEnumerable<string> inputList)
        {
            Dictionary<int, List<string>> encodedStrings = new Dictionary<int, List<string>>();
            foreach (string currString in inputList)
            {
                int encodedStringValue = EncodeStringWithPrimes(currString);
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
            return outputList;
        }

        private int EncodeStringWithPrimes(string currString)
        {
            int encodedStringValue = 1;
            foreach (char c in currString)
            {
                int charValue;
                if (!characterPrimeValues.TryGetValue(c, out charValue))
                {
                    int prime = PrimeGenerator.GetNextPrime();
                    characterPrimeValues.Add(c, prime);
                    charValue = prime;
                }
                encodedStringValue *= charValue;
            }
            return encodedStringValue;
        }
    }
}
