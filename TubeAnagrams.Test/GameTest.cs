using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using Xunit;
using static System.String;

namespace tubeAnagrams.Test
{
    public class GameTest
    {
        [Fact]
        public void AsksWhatTubeLineYouWantToGetAnagramsForAndLetsYouTypeInAnAnswer()
        {            
            const string chosenLine = "Central";

            var mockTflApi = new Mock<TflApi>();
            mockTflApi.Setup(mockTfl => mockTfl.GetStationsForLine(chosenLine)).Returns(new string[] {"Shepherds Bush", "White City"});
            
            var game = new Game(mockTflApi.Object);
            
            using (var sw = new StringWriter())
            using (var sr = new StringReader(chosenLine))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);
                
                game.Play();
                
                var consoleOutput = sw.ToString().Split("\n");

                Assert.Equal("What tube line do you want anagrams for?", consoleOutput[0]);
                Assert.Equal($"Unscramble these anagrams of stops along the {chosenLine} Line:", consoleOutput[1]);
            }
        }

        [Fact]
        public void GivesAnAnagramForAStationOnTheGivenTubeLine()
        {
            const string chosenLine = "Central";
            
            var mockTflApi = new Mock<TflApi>();
            var mockReturnStations = new[] {"Shepherds Bush"};
            mockTflApi.Setup(mockTfl => mockTfl.GetStationsForLine(chosenLine)).Returns(mockReturnStations);
            
            var game = new Game(mockTflApi.Object);
            
            using (var sw = new StringWriter())
            using (var sr = new StringReader(chosenLine))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);
                
                game.Play();
                
                var consoleOutput = sw.ToString().Split("\n");

                var stationOriginal = mockReturnStations[0];
                var stationAnagram = consoleOutput[2];
                Assert.True(TestHelpers.AreAnagrams(stationOriginal, stationAnagram));
            }
        }
    }
}