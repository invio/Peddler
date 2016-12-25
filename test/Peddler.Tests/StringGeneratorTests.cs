using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
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
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        public static IEnumerable<object[]> NextDistinct_Defaults_Data {
            get {
                yield return ToArray(String.Empty);
                yield return ToArray(" ");
                yield return ToArray("foo");
                yield return ToArray(new String('a', 256));
                yield return ToArray(new String('a', 10000));
                yield return ToArray(new String(CharacterSets.AsciiExtended.First(), 5));
            }
        }

        [Theory]
        [MemberData(nameof(NextDistinct_Defaults_Data))]
        public void NextDistinct_Defaults(String other) {
            var characters = CharacterSets.AsciiPrintable;
            var generator = new StringGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                Assert.NotEqual(value, other);
                Assert.InRange(value.Length, 0, 255);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
                Assert.False(generator.EqualityComparer.Equals(other, value));
            }
        }

        [Fact]
        public void NextDistinct_NullOther() {
            const string other = null;
            var generator = new StringGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                Assert.NotEqual(value, other);
                Assert.InRange(value.Length, 0, 255);
                Assert.False(generator.EqualityComparer.Equals(other, value));
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
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        [Theory]
        [InlineData(3, "foo")]
        [InlineData(5, "foo")]
        [InlineData(2, "foobar")]
        [InlineData(100, "barbizbuzzbooze")]
        [InlineData(0, "a")]
        [InlineData(3, "aa")]
        [InlineData(2, "aaa")]
        public void NextDistinct_WithLength(int length, String other) {
            var characters = CharacterSets.AsciiPrintable;
            var generator = new StringGenerator(length);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                Assert.NotEqual(value, other);
                Assert.Equal(length, value.Length);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
                Assert.False(generator.EqualityComparer.Equals(other, value));
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
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        public static IEnumerable<object[]> NextDistinct_WithLengthAndCharacters_Data {
            get {
                yield return ToArray(3, alphabet, "foo");
                yield return ToArray(5, new HashSet<Char> { 'a' }, "foo");
                yield return ToArray(2, new HashSet<Char> { 'a' }, "foobar");
                yield return ToArray(100, CharacterSets.AsciiPrintable, "barbizbuzzbooze");
                yield return ToArray(0, new HashSet<Char>{ 'a' }, "a");    // => ""
                yield return ToArray(3, new HashSet<Char>{ 'a' }, "aa");   // => "aaa"
                yield return ToArray(2, new HashSet<Char>{ 'a' }, "aaa");  // => "aa"
                yield return ToArray(3, new HashSet<Char>{ 'a' }, "aab");  // => "aaa"
                yield return ToArray(3, new HashSet<Char>{ 'a' }, "aba");  // => "aaa"
                yield return ToArray(3, new HashSet<Char>{ 'a' }, "baa");  // => "aaa"
            }
        }

        [Theory]
        [MemberData(nameof(NextDistinct_WithLengthAndCharacters_Data))]
        public void NextDistinct_WithLengthAndCharacters(
            int length,
            ISet<Char> characters,
            String other) {

            var generator = new StringGenerator(length, characters);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                Assert.NotEqual(value, other);
                Assert.Equal(length, value.Length);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
                Assert.False(generator.EqualityComparer.Equals(other, value));
            }
        }

        [Fact]
        public void NextDistinct_WithLengthAndCharacters_UnableToGenerateValueException() {
            var length = 1;
            var characters = new HashSet<Char> { 'a' };
            var other = "a";

            var generator = new StringGenerator(length, characters);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(other)
            );
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
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }


        [Theory]
        [InlineData(0, 100, "foo")]
        [InlineData(100, 200, "barbizbuzzbooze")]
        [InlineData(0, 10, "aaaaaaaaaa")]
        [InlineData(0, 0, "a")]
        [InlineData(2, 3, "aa")]
        [InlineData(2, 3, "aaa")]
        [InlineData(3, 3, "aab")]
        [InlineData(3, 3, "aba")]
        [InlineData(3, 3, "baa")]
        public void NextDistinct_WithRange(int minimum, int maximum, String other) {
            var characters = CharacterSets.AsciiPrintable;
            var generator = new StringGenerator(minimum, maximum);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                Assert.NotEqual(value, other);
                Assert.InRange(value.Length, minimum, maximum);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
                Assert.False(generator.EqualityComparer.Equals(other, value));
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
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        public static IEnumerable<object[]> NextDistinct_WithRangeAndCharacters_Data {
            get {
                yield return ToArray(0, 100, alphabet, "foo");
                yield return ToArray(5, 10, new HashSet<Char> { 'a' }, "foo");
                yield return ToArray(1, 2, new HashSet<Char> { 'a' }, "foobar");
                yield return ToArray(100, 200, CharacterSets.AsciiPrintable, "barbizbuzzbooze");
                yield return ToArray(0, 100, new HashSet<Char> { 'a' }, new String('a', 100));
                yield return ToArray(0, 0, new HashSet<Char>{ 'a' }, "a");    // => ""
                yield return ToArray(2, 3, new HashSet<Char>{ 'a' }, "aa");   // => "aaa"
                yield return ToArray(2, 3, new HashSet<Char>{ 'a' }, "aaa");  // => "aa"
                yield return ToArray(3, 3, new HashSet<Char>{ 'a' }, "aab");  // => "aaa"
                yield return ToArray(3, 3, new HashSet<Char>{ 'a' }, "aba");  // => "aaa"
                yield return ToArray(3, 3, new HashSet<Char>{ 'a' }, "baa");  // => "aaa"
            }
        }

        [Theory]
        [MemberData(nameof(NextDistinct_WithRangeAndCharacters_Data))]
        public void NextDistinct_WithRangeAndCharacters(
            int minimum,
            int maximum,
            ISet<Char> characters,
            String other) {

            var generator = new StringGenerator(minimum, maximum, characters);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                Assert.NotEqual(value, other);
                Assert.InRange(value.Length, minimum, maximum);
                Assert.DoesNotContain(value, character => !characters.Contains(character));
                Assert.False(generator.EqualityComparer.Equals(other, value));
            }
        }

        [Fact]
        public void NextDistinct_WithRangeAndCharacters_UnableToGenerateValueException() {
            var length = 1;
            var characters = new HashSet<Char> { 'a' };
            var other = "a";

            var generator = new StringGenerator(length, length, characters);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(other)
            );
        }

        [Fact]
        public async Task ThreadSafety() {

            // Arrange

            var generator = new StringGenerator(0, 1000);
            var consecutiveEmptyStrings = new ConcurrentStack<String>();

            Action createThread =
                () => this.ThreadSafetyImpl(generator, consecutiveEmptyStrings);

            // Act

            var threads =
                Enumerable
                    .Range(0, 10)
                    .Select(_ => Task.Run(createThread))
                    .ToArray();

            await Task.WhenAll(threads);

            // Assert

            Assert.True(
                consecutiveEmptyStrings.Count < 50,
                $"System.Random is not thread safe. If one of its .Next() " +
                $"implementations is called simultaneously on several " +
                $"threads, it breaks and starts returning zero exclusively. " +
                $"The last {consecutiveEmptyStrings.Count:N0} values were " +
                $"the empty string, signifying its internal System.Random " +
                $"is in a broken state."
            );
        }

        private void ThreadSafetyImpl(
            IGenerator<String> generator,
            ConcurrentStack<String> consecutiveEmptyStrings) {

            var count = 0;

            while (count++ < 10000) {
                if (generator.Next().Equals(String.Empty)) {
                    consecutiveEmptyStrings.Push(String.Empty);
                } else {
                    consecutiveEmptyStrings.Clear();
                }
            }
        }

        // Syntactic sugar.
        // 'yield return ToArray( ... )' is shorter than 'yield return new object[] { ... }'
        private static object[] ToArray(params object[] objects) {
            return objects;
        }

    }

}
