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
        public static ISet<Char> AsciiControl { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII printable characters.
        /// </summary>
        public static ISet<Char> AsciiPrintable { get; }

        /// <summary>
        ///   An immutable <see cref="ISet{Char}" /> of ASCII printable characters.
        /// </summary>
        public static ISet<Char> AsciiExtended { get; }

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
        }

    }

}
