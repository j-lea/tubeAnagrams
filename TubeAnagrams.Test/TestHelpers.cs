using System;
using System.Linq;
using static System.String;

namespace tubeAnagrams.Test
{
    public static class TestHelpers
    {
        public static bool AreAnagrams(string a, string b)
        {
            var aOrdered = Concat(a.ToUpper().OrderBy(c => c));
            var bOrdered = Concat(b.ToUpper().OrderBy(c => c));
            return aOrdered.Equals(bOrdered, StringComparison.OrdinalIgnoreCase);
        }
    }
}