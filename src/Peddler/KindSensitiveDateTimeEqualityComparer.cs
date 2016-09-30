using System;
using System.Collections.Generic;
using Invio.Hashing;

namespace Peddler {

    /// <summary>
    ///   An <see cref="Comparer{DateTime}" /> implementation that only considers
    ///   <see cref="DateTime" /> instances equal if they have the same
    ///   <see cref="DateTimeKind" />.
    /// </summary>
    public class KindSensitiveDateTimeEqualityComparer : EqualityComparer<DateTime> {

        /// <summary>
        ///   Gets the hash code that can be used to compare <see cref="DateTime" />
        ///   instances to see if they can be equal to one another.
        /// </summary>
        /// <remarks>
        ///   If two <see cref="DateTime" /> instances do not share the same hash code,
        ///   they will always be considered unequal.
        /// </remarks>
        /// <param name="input">
        ///   An instance of <see cref="DateTime" /> for which the caller wishes to
        ///   retrieve the hash code.
        /// </param>
        /// <returns>
        ///   The hash code value for this <see cref="DateTime" />.
        /// </returns>
        public override int GetHashCode(DateTime input) {
            return HashCode.From(input.Kind, input.Ticks);
        }

        /// <summary>
        ///   Verifies that the left and right are of the same <see cref="DateTimeKind" />,
        ///   then compares their internal ticks to determine if one is equal to the other.
        /// </summary>
        /// <param name="left">
        ///   An instance of <see cref="DateTime" /> the caller wishes to compare with
        ///   <paramref name="right" />.
        /// </param>
        /// <param name="right">
        ///   An instance of <see cref="DateTime" /> the caller wishes to compare with
        ///   <paramref name="left" />.
        /// </param>
        /// <returns>
        ///   <code>true</code> if <paramref name="left" /> is the same instant and
        ///   <see cref="DateTimeKind" /> as <paramref name="right" />.
        ///   Otherwise, <code>false</code>.
        /// </returns>
        public override bool Equals(DateTime left, DateTime right) {
            return left.Kind == right.Kind && left.Ticks == right.Ticks;
        }

    }

}
