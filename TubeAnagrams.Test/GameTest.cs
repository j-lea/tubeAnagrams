using System;
using System.IO;
using Moq;
using Xunit;

namespace tubeAnagrams.Test
{
    public class GameTest : IDisposable
    {
        private const string ChosenLine = "Central";
        private static readonly string[] MockReturnStations = {"Shepherds Bush", "White City"};

        private readonly StringWriter _sw;
        private readonly StringReader _sr;

        public GameTest()
        {
            var mockTflApi = new Mock<ITflApi>();
            mockTflApi.Setup(mockTfl => mockTfl.GetStationsForLine(It.IsAny<string>())).Returns(MockReturnStations);

            var sequentialPicker = new SequentialPicker();

            _sw = new StringWriter();
            _sr = new StringReader(ChosenLine);
            
            Console.SetOut(_sw);
            Console.SetIn(_sr);
            
            var game = new Game(mockTflApi.Object, sequentialPicker);
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

            Assert.True(TestHelpers.AreAnagrams(consoleOutput[2], MockReturnStations[0]));
        }

        [Fact]
        public void MakesYouTryAgainWhenYouGetItWrong()
        {
            
        }

        private class SequentialPicker : IPicker<string>
        {
            private int _index;

            public SequentialPicker()
            {
                this._index = 0;
            }

            public string Pick(string[] items)
            {
                return items[_index++];
            }
        }

        public void Dispose()
        {
            _sw.Close();
            _sr.Close();
        }
    }
}