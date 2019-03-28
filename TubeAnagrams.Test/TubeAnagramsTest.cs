using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Xunit;
using tubeAnagrams;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using Moq;
using MockServerClientNet;
using MockServerClientNet.Model;
using Times = Moq.Times;

namespace tubeAnagrams.Test
{
    public class TubeAnagramsTest
    {

        [Fact]
        public void AsksWhatTubeLineYouWantToGetAnagramsForAndLetsYouTypeInAnAnswer()
        {            
            const string chosenLine = "Central";
            const string responseBody = "[{\"commonName\":\"Bethnal Green Underground Station\"}, " +
                                        "{\"commonName\":\"White City Underground Station\"}]";
            
            const string mockApiUrl = "127.0.0.1";
            const int mockApiPort = 1080;
            
            SetUpMockServer(mockApiUrl, mockApiPort, responseBody);
            var tflApi = new TflApi($"http://{mockApiUrl}:{mockApiPort}");
            
            using (var sw = new StringWriter())
            using (var sr = new StringReader(chosenLine))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);
                
                new TubeAnagrams(tflApi);
                
                var consoleOutput = sw.ToString().Split("\n");

                Assert.Equal("What tube line do you want anagrams for?", consoleOutput[0]);
                Assert.Equal($"Unscramble these anagrams of stops along the {chosenLine} Line:", consoleOutput[1]);

                var anagram = consoleOutput[2];
                Assert.True(TestHelpers.AreAnagrams("Bethnal Green", anagram) ||
                            TestHelpers.AreAnagrams("White City", anagram));
            }
        }

        private void SetUpMockServer(string url, int port, string responseBody)
        {
            var msClient = new MockServerClient(url, port);
            msClient.Reset();

            msClient.When(HttpRequest.Request()
                        .WithMethod("GET")
                        .WithPath("/line/central/stoppoints"),
                    MockServerClientNet.Model.Times.Unlimited())
                .Respond(HttpResponse.Response()
                    .WithStatusCode(200)
                    .WithDelay(new TimeSpan())
                    .WithHeaders(new Header("Content-Type", "application/json"))
                    .WithBody(responseBody));

        }
    }
}