using System;
using Xunit;

namespace tubeAnagrams.Test
{
    public class AnagramGeneratorTest
    {
        [Fact]
        public void CreatesAnAnagramOfAWord()
        {
            const string word = "Tottenham Court Road";
            var anagram = AnagramGenerator.GenerateAnagram(word);

            Assert.True(TestHelpers.AreAnagrams(word, anagram));
            Assert.NotEqual(word, anagram);
        }

        [Fact]
        public void AnagramsAreUpperCase()
        {
            const string word = "Oxford Circus";
            var anagram = AnagramGenerator.GenerateAnagram(word);

            Assert.Equal(anagram.ToUpper(), anagram);
        }
    }
}