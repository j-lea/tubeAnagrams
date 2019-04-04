using System;
using System.Linq;
using static System.String;

namespace tubeAnagrams
{
    public class Game
    {
        private readonly ITflApi _tflApi;
        private readonly IPicker<string> _picker;
        
        public Game(ITflApi tflApi, IPicker<string> picker)
        {
            _tflApi = tflApi;
            _picker = picker;
        }

        public void Play()
        {
            Console.WriteLine("What tube line do you want anagrams for?");

            var chosenTubeLine = Console.ReadLine();
            
            Console.WriteLine($"Unscramble these anagrams of stops along the {chosenTubeLine} Line:");

            var stationsForLine = _tflApi.GetStationsForLine(chosenTubeLine);

            var anagram = AnagramGenerator.GenerateAnagram(_picker.Pick(stationsForLine));
            Console.WriteLine(anagram);

            var guess = Console.ReadLine();
            
            if (guess != null && AreAnagrams(anagram, guess))
            {
                Console.WriteLine("Correct!");
            }
            else
            {
                Console.WriteLine($"No {guess} is wrong.");
            }
        }
        
        private static bool AreAnagrams(string a, string b)
        {
            var aOrdered = Concat(a.ToUpper().OrderBy(c => c));
            var bOrdered = Concat(b.ToUpper().OrderBy(c => c));
            return aOrdered.Equals(bOrdered, StringComparison.OrdinalIgnoreCase);
        }
    }
}