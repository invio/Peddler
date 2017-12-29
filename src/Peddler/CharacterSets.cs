using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Peddler {

    /// <summary>
    ///   Various immutable collections of characters that can be used for
    ///   <see cref="Char" /> or <see cref="String" /> generation.
    /// </summary>
    public static class CharacterSets {

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII control characters.
        /// </summary>
        /// <remarks>
        ///   Source: https://en.wikipedia.org/wiki/ASCII#Control_characters
        /// </remarks>
        public static ISet<Char> AsciiControl { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII printable characters.
        /// </summary>
        /// <remarks>
        ///   Source: https://en.wikipedia.org/wiki/ASCII#Printable_characters
        /// </remarks>
        public static ISet<Char> AsciiPrintable { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII printable characters.
        /// </summary>
        /// <remarks>
        ///   Source: https://en.wikipedia.org/wiki/Extended_ASCII
        /// </remarks>
        public static ISet<Char> AsciiExtended { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of alphabetical ASCII characters.
        /// </summary>
        public static ISet<Char> AsciiAlphabetical { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII numeric characters.
        /// </summary>
        public static ISet<Char> AsciiNumeric { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII alphanumeric characters.
        /// </summary>
        public static ISet<Char> AsciiAlphanumeric { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII characters that are
        ///   not reserved for some functional purpose.
        /// </summary>
        /// <remarks>
        ///   This list is plucked out the of RFC 3986, which defines valid characters
        ///   that can be used in a URL that are "unreserved" for any functional purpose,
        ///   such as a question mark for query strings, or a number sign for anchors.
        ///   Source: https://tools.ietf.org/html/rfc3986#section-2.3
        /// </remarks>
        public static ISet<Char> AsciiUrlUnreserved { get; }

        static CharacterSets() {
            AsciiControl =
                Enumerable
                    .Range(0, 32)            // Results in 0 - 31
                    .Concat(new [] { 127 })  // Don't forget DEL character!
                    .Select(Convert.ToChar)
                    .ToImmutableHashSet();

            AsciiPrintable =
                Enumerable
                    .Range(32, 95)
                    .Select(Convert.ToChar)
                    .ToImmutableHashSet();

            AsciiExtended =
                Enumerable
                    .Range(128, 128)
                    .Select(Convert.ToChar)
                    .ToImmutableHashSet();

            AsciiAlphabetical =
                Enumerable
                    .Range('a', 26)
                    .Concat(Enumerable.Range('A', 26))
                    .Select(Convert.ToChar)
                    .ToImmutableHashSet();

            AsciiNumeric =
                Enumerable
                    .Range('0', 10)
                    .Select(Convert.ToChar)
                    .ToImmutableHashSet();

            AsciiAlphanumeric =
                AsciiAlphabetical
                    .Concat(AsciiNumeric)
                    .ToImmutableHashSet();

            AsciiUrlUnreserved =
                AsciiAlphanumeric
                    .Concat(new Char[] { '-', '.', '_', '~' })
                    .ToImmutableHashSet();
        }

    }

}
