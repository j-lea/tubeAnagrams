using System;
using System.Linq;
using static System.String;

namespace tubeAnagrams
{
    public static class AnagramGenerator
    {
        public static string GenerateAnagram(string originalWord)
        {
            var random = new Random();
            return Concat(originalWord.OrderBy(x => random.Next())).ToUpper();
        }
    }
}