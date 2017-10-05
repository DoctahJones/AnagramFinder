using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anagrams
{
    public interface IPrimeGenerator
    {
        int CurrentPrime { get; }
        int CurrentPrimeIndex { get; set; }

        int GetNextPrime();

        int GetNthPrime(int n);
    }
}
