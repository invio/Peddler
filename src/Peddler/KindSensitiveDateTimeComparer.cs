using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   An <see cref="Comparer{DateTime}" /> implementation that only allows comparisons of
    ///   <see cref="DateTime" /> instances if they have the same <see cref="DateTimeKind" />.
    /// </summary>
    public class KindSensitiveDateTimeComparer : Comparer<DateTime> {

        private long ticksPerUnit { get; }

        /// <summary>
        ///   Creates an implementation of <see cref="Comparer{DateTime}" /> implementation
        ///   that only allows comparisons of <see cref="DateTime" /> instances if they have
        ///   the same <see cref="DateTimeKind" />.
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
        public KindSensitiveDateTimeComparer(DateTimeUnit granularity = DateTimeUnit.Tick) {
            this.ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);
        }

        /// <summary>
        ///   Verifies that the left and right are of the same <see cref="DateTimeKind" />,
        ///   then compares their internal ticks to determine if one is less than, greater
        ///   than, or equal to the other.
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
        ///   -1 if <paramref name="left" /> is earlier than <paramref name="right" />,
        ///   0 if <paramref name="left" /> is the same instant as <paramref name="right" />,
        ///   and 1 if <paramref name="left" /> is later than <paramref name="right" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="DateTimeKind" /> on the <paramref name="left" />
        ///   parameter is different than the <see cref="DateTimeKind" /> specified on the
        ///   <paramref name="right" /> parameter.
        /// </exception>
        public override int Compare(DateTime left, DateTime right) {
            if (left.Kind != right.Kind) {
                throw new ArgumentException(
                    $"The {typeof(DateTimeKind).Name} of '{nameof(left)}' ({left.Kind:G}) " +
                    $"does not match the {typeof(DateTimeKind).Name} of '{nameof(right)}' " +
                    $"({right.Kind:G}), so they cannot be compared."
                );
            }


            return (left.Ticks / this.ticksPerUnit).CompareTo(right.Ticks / this.ticksPerUnit);
        }

    }

}
