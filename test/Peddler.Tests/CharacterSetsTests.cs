using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Xunit;

namespace Peddler {

    public class CharacterSetsTests {

        [Fact]
        public void AsciiControl_IsControl() {
            foreach (var character in CharacterSets.AsciiControl) {
                Assert.Equal(
                    UnicodeCategory.Control,
                    CharUnicodeInfo.GetUnicodeCategory(character)
                );
            }
        }

        [Fact]
        public void AsciiControl_ContainsDeleteAndZeroTo31() {
            const Char DEL = '\u007f';

            Assert.Contains(DEL, CharacterSets.AsciiControl);

            var filtered =
                CharacterSets
                    .AsciiControl
                    .Where(character => character != DEL)
                    .Select(Convert.ToInt32);

            foreach (var character in filtered) {
                Assert.True(character >= 0, $"Unexpected character '\\u{character:x4}'.");
                Assert.True(character <= 31, $"Unexpected character '\\u{character:x4}'.");
            }
        }

        [Fact]
        public void AsciiPrintable_Ranges32To126() {
            var characters =
                CharacterSets
                    .AsciiPrintable
                    .Select(Convert.ToInt32);

            Assert.Contains(32, characters);
            Assert.Contains(126, characters);

            foreach (var character in characters) {
                Assert.True(character >= 32, $"Unexpected character '\\u{character:x4}'.");
                Assert.True(character <= 126, $"Unexpected character '\\u{character:x4}'.");
            }
        }

        [Fact]
        public void AsciiExtended_Ranges128To257() {
            var characters =
                CharacterSets
                    .AsciiExtended
                    .Select(Convert.ToInt32);

            Assert.Contains(128, characters);
            Assert.Contains(255, characters);

            foreach (var character in characters) {
                Assert.True(character >= 128, $"Unexpected character '\\u{character:x4}'.");
                Assert.True(character <= 255, $"Unexpected character '\\u{character:x4}'.");
            }
        }

        [Fact]
        public void AsciiAlphabetical_IsLetterInAlphabet() {
             Assert.Equal(26 * 2, CharacterSets.AsciiAlphabetical.Count);

            foreach (var character in CharacterSets.AsciiAlphabetical) {
                Assert.True(
                    Char.IsLetter(character),
                    $"Unexpected character '\\u{character:x4}'."
                );

                if (Char.IsLower(character)) {
                    Assert.True(
                        'a' <= character && character <= 'z',
                        $"Unexpected character '{character}'."
                    );
                } else {
                    Assert.True(
                        Char.IsUpper(character),
                        $"Unexpected character '{character}'."
                    );

                    Assert.True(
                        'A' <= character && character <= 'Z',
                        $"Unexpected character '{character}'."
                    );
                }
            }
        }

        public static IEnumerable<object[]> CharacterSets_IsImmutable_Data {
            get {
                yield return new [] { CharacterSets.AsciiControl };
                yield return new [] { CharacterSets.AsciiPrintable };
                yield return new [] { CharacterSets.AsciiExtended };
                yield return new [] { CharacterSets.AsciiAlphabetical };
            }
        }

        [Theory]
        [MemberData(nameof(CharacterSets_IsImmutable_Data))]
        public void CharacterSets_IsImmutable(ISet<Char> characters) {
            Assert.Throws<NotSupportedException>(
                () => characters.Add('A')
            );
        }


    }

}
