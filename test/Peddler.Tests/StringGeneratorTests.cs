using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace Peddler {

    public class StringGeneratorTests {

        private const int numberOfAttempts = 100;
        private static ISet<Char> empty { get; }
        private static ISet<Char> alphabet { get; }

        static StringGeneratorTests() {
            empty = ImmutableHashSet<Char>.Empty;

            var lowerCaseAlphabet = new HashSet<Char> {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
            };

            alphabet = lowerCaseAlphabet.ToImmutableHashSet();
        }

        [Fact]
        public void Constructor_WithLength_RequiresNonNegative() {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new StringGenerator(-1)
            );
        }

        [Fact]
        public void Constructor_WithLengthAndCharacters_RequiresNonNullChars() {
            Assert.Throws<ArgumentNullException>(
                () => new StringGenerator(10, null)
            );
        }

        [Fact]
        public void Constructor_WithLengthAndCharacters_RequiresNonNegative() {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new StringGenerator(-1, alphabet)
            );
        }

        [Fact]
        public void Constructor_WithLengthAndCharacters_RequiresCharsWhenNotEmpty() {
            Assert.Throws<ArgumentException>(
                () => new StringGenerator(5, empty)
            );
        }

        [Fact]
        public void Constructor_WithLengthAndCharacters_AllowsEmptyCharsWithLengthOfZero() {
            var generator = new StringGenerator(0, empty);

            Assert.Equal(String.Empty, generator.Next());
        }

        [Fact]
        public void Constructor_WithRange_RequiresNonNegative() {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new StringGenerator(-1, 10)
            );
        }

        [Fact]
        public void Constructor_WithRange_RequiresLowerMinimum() {
            Assert.Throws<ArgumentException>(
                () => new StringGenerator(10, 9)
            );
        }

        [Fact]
        public void Constructor_WithRangeAndCharacters_RequiresNonNullChars() {
            Assert.Throws<ArgumentNullException>(
                () => new StringGenerator(1, 10, null)
            );
        }

        [Fact]
        public void Constructor_WithRangeAndCharacters_RequiresNonNegative() {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new StringGenerator(-1, 10, alphabet)
            );
        }

        [Fact]
        public void Constructor_WithRangeAndCharacters_RequiresLowerMinimum() {
            Assert.Throws<ArgumentException>(
                () => new StringGenerator(5, 4, alphabet)
            );
        }

        [Fact]
        public void Constructor_WithRangeAndCharacters_RequiresCharsWhenNotEmpty() {
            Assert.Throws<ArgumentException>(
                () => new StringGenerator(1, 5, empty)
            );
        }

        [Fact]
        public void Constructor_WithRangeAndCharacters_AllowsEmptyCharsWithMaximumIsZero() {
            var generator = new StringGenerator(0, 0, empty);

            Assert.Equal(String.Empty, generator.Next());
        }

        [Fact]
        public void Next_Defaults() {
            var characters = CharacterSets.AsciiPrintable;
            var generator = new StringGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.InRange(value.Length, 0, 255);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void Next_WithLength(int length) {
            var characters = CharacterSets.AsciiPrintable;
            var generator = new StringGenerator(length);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.Equal(length, value.Length);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void Next_WithLengthAndCharacters(int length) {
            var generator = new StringGenerator(length, alphabet);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.Equal(length, value.Length);
                Assert.DoesNotContain(value, character => !alphabet.Contains(character));
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 100)]
        [InlineData(100, 100)]
        [InlineData(100, 1000)]
        public void Next_WithRange(int minimum, int maximum) {
            var characters = CharacterSets.AsciiPrintable;
            var generator = new StringGenerator(minimum, maximum);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.InRange(value.Length, minimum, maximum);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 100)]
        [InlineData(100, 100)]
        [InlineData(100, 1000)]
        public void Next_WithRangeAndCharacters(int minimum, int maximum) {
            var generator = new StringGenerator(minimum, maximum, alphabet);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.InRange(value.Length, minimum, maximum);
                Assert.DoesNotContain(value, character => !alphabet.Contains(character));
            }
        }

    }

}
