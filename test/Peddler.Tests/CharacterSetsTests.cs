using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Globalization;
using Xunit;

namespace Peddler {

    public sealed class CharacterSetsTests {

        public static IEnumerable<object[]> AsciiControl { get; } =
            CharacterSets
                .AsciiControl
                .Select(character => new object[] { character })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(AsciiControl))]
        public void AsciiControl_IsControlCharacter(Char character) {

            // Arrange

            const UnicodeCategory expectedCategory = UnicodeCategory.Control;

            // Act

            var actualCategory = CharUnicodeInfo.GetUnicodeCategory(character);

            // Assert

            Assert.Equal(expectedCategory, actualCategory);
        }

        [Theory]
        [MemberData(nameof(AsciiControl))]
        public void AsciiControl_ContainsDeleteAndZeroTo31(Char character) {

            // Arrange

            const Char delete = '\u007f';

            // Act

            var isDelete = (character == delete);
            var isZeroToThirtyOne = (0 <= character && character <= 31);

            // Assert

            Assert.True(
                isDelete ^ isZeroToThirtyOne,
                $"Unexpected character '\\u{(int)character:x4}'."
            );
        }

        [Fact]
        public void AsciiPrintable_Contains95Characters() {

            // Arrange

            const int expectedCount = 95;  // Characters 32 - 126, inclusively

            // Act

            var actualCount = CharacterSets.AsciiPrintable.Count;

            // Assert

            Assert.Equal(expectedCount, actualCount);
        }

        public static IEnumerable<object[]> AsciiPrintable { get; } =
            CharacterSets
                .AsciiPrintable
                .Select(character => new object[] { character })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(AsciiPrintable))]
        public void AsciiPrintable_Ranges32To126(Char character) {

            // Act

            var isThirtyTwoToOneHundredTwentySix = (32 <= character && character <= 126);

            // Assert

            Assert.True(
                isThirtyTwoToOneHundredTwentySix,
                $"Unexpected character '\\u{(int)character:x4}'."
            );
        }

        [Fact]
        public void AsciiExtended_Contains128Characters() {

            // Act

            const int expectedCount = 128;  // Characters 128 - 255, inclusively

            // Act

            var actualCount = CharacterSets.AsciiExtended.Count;

            // Assert

            Assert.Equal(expectedCount, actualCount);
        }

        public static IEnumerable<object[]> AsciiExtended { get; } =
            CharacterSets
                .AsciiExtended
                .Select(character => new object[] { character })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(AsciiExtended))]
        public void AsciiExtended_Ranges128To257(Char character) {

            // Act

            var isInExtendedAsciiRange = (128 <= character && character <= 255);

            // Assert

            Assert.True(
                isInExtendedAsciiRange,
                $"Unexpected character '\\u{(int)character:x4}'."
            );
        }

        [Fact]
        public void AsciiAlphabetical_Contains52Characters() {

            // Arrange

            const int expectedCount = 26 * 2; // 26 Lowercase, 26 Uppercase

            // Act

            var actualCount = CharacterSets.AsciiAlphabetical.Count;

            // Assert

            Assert.Equal(expectedCount, actualCount);
        }

        public static IEnumerable<Object[]> AsciiAlphabetical { get; } =
            CharacterSets
                .AsciiAlphabetical
                .Select(character => new object[] { character })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(AsciiAlphabetical))]
        public void AsciiAlphabetical_IsUpperOrLowerLetter(Char character) {

            // Act

            var isLetter = Char.IsLetter(character);
            var isLower = Char.IsLower(character);
            var isUpper = Char.IsUpper(character);

            // Assert

            Assert.True(
                isLetter,
                $"Unexpected non-letter character '\\u{(int)character:x4}'."
            );

            Assert.True(
                isLower ^ isUpper,
                $"Expected '{character}' to be EITHER lower or upper case - not both.\n" +
                $"  Char.IsLower('{character}') = {isLower}\n" +
                $"  Char.IsUpper('{character}') = {isUpper}"
            );
        }

        [Fact]
        public void AsciiNumeric_Contains10Characters() {

            // Arrange

            const int expectedCount = 10; // 0 - 9, inclusively

            // Act

            var actualCount = CharacterSets.AsciiNumeric.Count;

            // Assert

            Assert.Equal(expectedCount, actualCount);
        }

        public static IEnumerable<Object[]> AsciiNumeric { get; } =
            CharacterSets
                .AsciiNumeric
                .Select(character => new object[] { character })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(AsciiNumeric))]
        public void AsciiNumeric_ContainsOnlyDigits(Char character) {

            // Act

            var isDigit = Char.IsDigit(character);

            // Assert

            Assert.True(
                isDigit,
                $"Unexpected non-digit character '\\u{(int)character:x4}'."
            );
        }


        [Fact]
        public void AsciiAlphanumeric_Contains62Characters() {

            // Arrange

            const int expectedCount = 62; // 0 - 9 (inclusively), 26 lowercase, 26 uppercase

            // Act

            var actualCount = CharacterSets.AsciiAlphanumeric.Count;

            // Assert

            Assert.Equal(expectedCount, actualCount);
        }

        public static IEnumerable<Object[]> AsciiAlphanumeric { get; } =
            CharacterSets
                .AsciiAlphanumeric
                .Select(character => new object[] { character })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(AsciiAlphanumeric))]
        public void AsciiAlphanumeric_ContainsLettersOrDigits(Char character) {

            // Act

            var isDigit = Char.IsDigit(character);
            var isLetter = Char.IsLetter(character);

            // Assert

            Assert.True(
                isDigit || isLetter,
                $"Unexpected non-digit, non-letter character '\\u{(int)character:x4}'."
            );

            Assert.True(
                isDigit ^ isLetter,
                $"Expected '{character}' to be EITHER digit or letter - not both.\n" +
                $"  Char.IsLower('{character}') = {isDigit}\n" +
                $"  Char.IsUpper('{character}') = {isLetter}"
            );
        }

        [Fact]
        public void AsciiUrlUnreserved_Contains66Characters() {

            // Arrange

            // The following characters are expected:
            //   * 0 - 9, inclusively       (10 characters)
            //   * lowercase letters        (26 characters)
            //   * uppercase letters        (26 characters)
            //   * '-', '.', '_', '~'       ( 4 characters)
            const int expectedCount = 66;

            // Act

            var actualCount = CharacterSets.AsciiUrlUnreserved.Count;

            // Assert

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [MemberData(nameof(AsciiAlphanumeric))]
        [InlineData('-')]
        [InlineData('.')]
        [InlineData('_')]
        [InlineData('~')]
        public void AsciiUrlUnreserved_ActuallyContainsCorrectCharacters(Char character) {
            Assert.Contains(character, CharacterSets.AsciiUrlUnreserved);
        }

        public static IEnumerable<object[]> CharacterSets_IsImmutable_Data { get; } =
            ImmutableList<ISet<Char>>
                .Empty
                .Add(CharacterSets.AsciiControl)
                .Add(CharacterSets.AsciiPrintable)
                .Add(CharacterSets.AsciiExtended)
                .Add(CharacterSets.AsciiAlphabetical)
                .Add(CharacterSets.AsciiNumeric)
                .Add(CharacterSets.AsciiAlphanumeric)
                .Add(CharacterSets.AsciiUrlUnreserved)
                .Select(characters => new object[] { characters })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(CharacterSets_IsImmutable_Data))]
        public void CharacterSets_IsImmutable(ISet<Char> characters) {

            // Act

            var exception = Record.Exception(
                () => characters.Add('A')
            );

            // Assert

            Assert.IsType<NotSupportedException>(exception);
        }


    }

}
