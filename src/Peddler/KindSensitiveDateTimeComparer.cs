using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   An <see cref="Comparer{DateTime}" /> implementation that allows the caller
    ///   to restricted by the <see cref="DateTimeKind" />
    ///   caller to re
    /// </summary>
    public class KindSensitiveDateTimeComparer :
        Comparer<DateTime>, IEqualityComparer<DateTime> {

        /// <summary>
        ///   The <see cref="DateTimeKind" /> that will be checked on all <see cref="DateTime" />
        ///   objects compared with this <see cref="KindSensitiveDateTimeComparer" />. If a
        ///   caller tries to compare <see cref="DateTime" /> instances that are not of this
        ///   <see cref="DateTimeKind" />, an <see cref="ArgumentException" /> will be thrown.
        /// </summary>
        public DateTimeKind Kind { get; }

        /// <summary>
        ///   Creates a new instance of <see cref="KindSensitiveDateTimeComparer" /> that
        ///   will only allow comparisons
        /// </summary>
        /// <param name="kind">
        ///   The <see cref="DateTimeKind" /> this comparer will allow.
        /// </param>
        public KindSensitiveDateTimeComparer(DateTimeKind kind) {
            this.Kind = kind;
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
        ///   <code>true</code> if <paramref name="left" /> is the same instant
        ///   as <paramref name="right" />. Otherwise, <code>false</code>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="Kind" /> defined on this
        ///   <see cref="KindSensitiveDateTimeComparer" /> is different than the
        ///   <see cref="DateTimeKind" /> specified on the <paramref name="left" />
        ///   or <paramref name="right" /> parameter.
        /// </exception>
        public virtual bool Equals(DateTime left, DateTime right) {
            return this.Compare(left, right) == 0;
        }

        /// <summary>
        ///   Gets the hash code that would be used to compare <see cref="DateTime" />
        ///   instances to one another.
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
        ///   The default hash code value for this <see cref="DateTime" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="Kind" /> defined on this
        ///   <see cref="KindSensitiveDateTimeComparer" /> is different than the
        ///   <see cref="DateTimeKind" /> specified on the <paramref name="input" />
        ///   parameter.
        /// </exception>
        public virtual int GetHashCode(DateTime input) {
            if (input.Kind != this.Kind) {
                throw new ArgumentException(
                    $"The {typeof(DateTimeKind).Name} of '{nameof(input)}' ({input.Kind:G}) " +
                    $"does not match the {typeof(DateTimeKind).Name} of this " +
                    $"{typeof(KindSensitiveDateTimeComparer).Name} ({this.Kind:G}).",
                    nameof(input)
                );
            }

            return input.GetHashCode();
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
        ///   Thrown when the <see cref="Kind" /> defined on this
        ///   <see cref="KindSensitiveDateTimeComparer" /> is different than the
        ///   <see cref="DateTimeKind" /> specified on the <paramref name="left" />
        ///   or <paramref name="right" /> parameter.
        /// </exception>
        public override int Compare(DateTime left, DateTime right) {
            if (left.Kind != this.Kind) {
                throw new ArgumentException(
                    $"The {typeof(DateTimeKind).Name} of '{nameof(left)}' ({left.Kind:G}) " +
                    $"does not match the {typeof(DateTimeKind).Name} of this " +
                    $"{typeof(KindSensitiveDateTimeComparer).Name} ({this.Kind:G}).",
                    nameof(left)
                );
            }

            if (right.Kind != this.Kind) {
                throw new ArgumentException(
                    $"The {typeof(DateTimeKind).Name} of '{nameof(right)}' ({right.Kind:G}) " +
                    $"does not match the {typeof(DateTimeKind).Name} of this " +
                    $"{typeof(KindSensitiveDateTimeComparer).Name} ({this.Kind:G}).",
                    nameof(right)
                );
            }

            return left.CompareTo(right);
        }

    }

}
