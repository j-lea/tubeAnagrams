using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;
using JennysCSharpApp;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace JennysCSharpApp.Test
{
    public class TubeAnagramsTest
    {

        [Fact]
        public void AsksWhatTubeLineYouWantToGetAnagramsFor()
        {            
            
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                TubeAnagrams.Main(new string[] {});

                Assert.Equal("What tube line do you want anagrams for?", sw.ToString());
            }
            
        }
    }
}