using System;
using System.IO;
using Moq;
using Xunit;

namespace tubeAnagrams.Test
{
    public class GameTest : IDisposable
    {
        private readonly Mock<TflApi> _mockTflApi;

        private const string ChosenLine = "Central";
        private static readonly string[] MockReturnStations = new string[] {"Shepherds Bush", "White City"};

        private readonly StringWriter _sw;
        private readonly StringReader _sr;

        public GameTest()
        {
            _mockTflApi = new Mock<TflApi>(null);
            _mockTflApi.Setup(mockTfl => mockTfl.GetStationsForLine(ChosenLine)).Returns(MockReturnStations);

            _sw = new StringWriter();
            _sr = new StringReader(ChosenLine);
            
            Console.SetOut(_sw);
            Console.SetIn(_sr);
            
            var game = new Game(_mockTflApi.Object);
            game.Play();
        }

        [Fact]
        public void AsksWhatTubeLineYouWantToGetAnagramsForAndLetsYouTypeInAnAnswer()
        {            
            var consoleOutput = _sw.ToString().Split("\n");

            Assert.Equal("What tube line do you want anagrams for?", consoleOutput[0]);
            Assert.Equal($"Unscramble these anagrams of stops along the {ChosenLine} Line:", consoleOutput[1]);
        }

        [Fact]
        public void GivesAnAnagramForAStationOnTheGivenTubeLine()
        {
            var consoleOutput = _sw.ToString().Split("\n");

            var anagram = consoleOutput[2];
            Assert.True(
                TestHelpers.AreAnagrams(MockReturnStations[0], anagram) ||
                TestHelpers.AreAnagrams(MockReturnStations[1], anagram));
        }

        public void Dispose()
        {
            _sw.Close();
            _sr.Close();
        }
    }
}