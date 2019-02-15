using System;
using System.Collections.Generic;
using Invio.Hashing;

namespace Peddler {

    /// <summary>
    ///   An <see cref="Comparer{DateTime}" /> implementation that only considers
    ///   <see cref="DateTime" /> instances equal if they have the same
    ///   <see cref="DateTimeKind" />.
    /// </summary>
    public sealed class KindSensitiveDateTimeEqualityComparer : EqualityComparer<DateTime> {

        private long ticksPerUnit { get; }

        /// <summary>
        ///   Creates an <see cref="Comparer{DateTime}" /> implementation that only
        ///   considers <see cref="DateTime" /> instances equal if they have the same
        ///   <see cref="DateTimeKind" />.
        /// </summary>
        /// <param name="granularity">
        ///   How granular the comparisons are between two <see cref="DateTime" /> values
        ///   with the same <see cref="DateTimeKind" />. For example, a
        ///   <paramref name="granularity" /> of <see cref="DateTimeUnit.Day" />
        ///   will consider any two <see cref="DateTime" /> values on the same day to be
        ///   equivalent. However, a <paramref name="granularity" /> of
        ///   <see cref="DateTimeUnit.Second" /> will consider any two <see cref="DateTime" />
        ///   values for the same second to be equivalent. The default
        ///   <paramref name="granularity" /> is <see cref="DateTimeUnit.Tick" />, which
        ///   requires the number of ticks in each <see cref="DateTime" /> value to be
        ///   identical in order for them to be considered equal.
        /// </param>
        public KindSensitiveDateTimeEqualityComparer(
            DateTimeUnit granularity = DateTimeUnit.Tick) {

            this.ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);
        }

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
            return HashCode.From(input.Kind, input.Ticks / this.ticksPerUnit);
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
            return left.Kind == right.Kind
                && ((left.Ticks / this.ticksPerUnit) == (right.Ticks / this.ticksPerUnit));
        }

    }

}
