using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Xunit;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace tubeAnagrams.Test
{
    public class TubeAnagramsTest : IDisposable
    {
        private const string ChosenLine = "Central";

        private static readonly string[] ResponseStations =
            {"White City", "Shepherds Bush"};

        private readonly string _responseBody = GetResponseBody(ResponseStations);
        
        private readonly FluentMockServer _mockServer;

        private readonly TflApi _tflApi;

        public TubeAnagramsTest()
        {
            _mockServer = FluentMockServer.Start();
            
            _mockServer.Given(
                    Request.Create()
                        .WithPath("/line/central/stoppoints")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(_responseBody)
                );
            
            _tflApi = new TflApi(_mockServer.Urls[0]);
        }

        [Fact]
        public void CorrectAnswerFlow()
        {            
            const string correctGuess = "WHITE CITY";
            
            using (var sw = new StringWriter())
            using (var sr = new StringReader($"{ChosenLine}\n{correctGuess}"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);

                var ta = new TubeAnagrams(_tflApi);
                
                var consoleOutput = sw.ToString().Split("\n");

                Assert.Equal("What tube line do you want anagrams for?", consoleOutput[0]);
                Assert.Equal($"Unscramble these anagrams of stops along the {ChosenLine} Line:", consoleOutput[1]);

                var firstAnagram = consoleOutput[2];
                Assert.True(IsAValidAnagram(firstAnagram, ResponseStations));
                
                var answerResponse = consoleOutput[3];
                Assert.Equal("Correct!", answerResponse);

//                var secondAnagram = consoleOutput[4];
//                Assert.True(TestHelpers.AreAnagrams("Shepherds Bush", secondAnagram));
            }
        }

        [Fact]
        public void IncorrectAnswerFlow()
        {
            const string incorrectGuess = "XYZ";
            
            using (var sw = new StringWriter())
            using (var sr = new StringReader($"{ChosenLine}\n{incorrectGuess}"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);
                
                var ta = new TubeAnagrams(_tflApi);
                
                var consoleOutput = sw.ToString().Split("\n");

                Assert.Equal("What tube line do you want anagrams for?", consoleOutput[0]);
                Assert.Equal($"Unscramble these anagrams of stops along the {ChosenLine} Line:", consoleOutput[1]);

                var anagram = consoleOutput[2];
                Assert.True(IsAValidAnagram(anagram, ResponseStations));

                var answerResponse = consoleOutput[3];
                Assert.Equal($"No {incorrectGuess} is wrong.", answerResponse);
            }
        }

        public void Dispose()
        {
            _mockServer.Stop();
            
            var standardOutput = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true};
            Console.SetOut(standardOutput);
        }

        private static string GetResponseBody(string[] stations)
        {
            var formattedStations = stations.Select(s => "{\"commonName\":\"" + s + " Underground Station\"}").ToArray();

            var responseBody = string.Join(',', formattedStations);
            return $"[{responseBody}]";
        }

        private bool IsAValidAnagram(string anagram, string[] stations)
        {
            return stations.Any(station => TestHelpers.AreAnagrams(station, anagram));
        }
    }
}