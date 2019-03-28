using System;

namespace tubeAnagrams
{
    public class Game
    {
        private readonly ITflApi _tflApi;
        
        public Game(ITflApi tflApi)
        {
            _tflApi = tflApi;
        }

        public void Play()
        {
            Console.WriteLine("What tube line do you want anagrams for?");

            var chosenTubeLine = Console.ReadLine();
            
            Console.WriteLine($"Unscramble these anagrams of stops along the {chosenTubeLine} Line:");

            var stationsForLine = _tflApi.GetStationsForLine(chosenTubeLine);
            
            var random = new Random();
            var anagram = AnagramGenerator.GenerateAnagram(stationsForLine[random.Next(stationsForLine.Length)]);
            Console.WriteLine(anagram);
        }
    }
}