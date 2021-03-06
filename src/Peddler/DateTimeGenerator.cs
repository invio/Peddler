using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Peddler {

    /// <summary>
    ///   A generator for values of type <see cref="DateTime" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="DateTime" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="DateTime" /> values will always be contiguous.
    ///   Each <see cref="DateTime" /> is considered distinct from another by
    ///   the number of ticks used to represent its value.
    /// </remarks>
    public sealed class DateTimeGenerator : IIntegralGenerator<DateTime> {

        private long ticksPerUnit { get; }
        private Int64Generator unitsGenerator { get; }

        /// <summary>
        ///   The <see cref="DateTimeKind" /> that must be specified for all
        ///   <see cref="DateTime" /> instances provided to this
        ///   <see cref="DateTimeGenerator" />. Additionally, all <see cref="DateTime" />
        ///   instances created by this <see cref="DateTimeGenerator" /> will have this
        ///   <see cref="DateTimeKind" /> specified.
        /// </summary>
        public DateTimeKind Kind { get; }

        /// <summary>
        ///   Inclusively, the earliest possible <see cref="DateTime" />
        ///   that can be created by this <see cref="DateTimeGenerator" />.
        /// </summary>
        public DateTime Low { get; }

        /// <summary>
        ///   Exclusively, the latest possible <see cref="DateTime" />
        ///   that can be created by this <see cref="DateTimeGenerator" />.
        /// </summary>
        public DateTime High { get; }

        /// <summary>
        ///   The comparison used to determine if two <see cref="DateTime" />
        ///   instances are equal in value.
        /// </summary>
        public IEqualityComparer<DateTime> EqualityComparer { get; }

        /// <summary>
        ///   The comparison used to determine if one <see cref="DateTime" />
        ///   instance is earlier than, the same time as, or later than another
        ///   <see cref="DateTime" /> instance.
        /// </summary>
        public IComparer<DateTime> Comparer { get; }

        /// <summary>
        ///   Instantiates a <see cref="DateTimeGenerator" /> that can create
        ///   <see cref="DateTime" /> values which range from
        ///   <see cref="DateTime.MinValue" /> to <see cref="DateTime.MaxValue" />.
        ///   The resultant <see cref="DateTimeGenerator" /> will expect and return
        ///   <see cref="DateTime" /> objects with a <see cref="DateTimeKind" />
        ///   of <see cref="DateTimeKind.Utc" />.
        /// </summary>
        /// <param name="granularity">
        ///   How granular generated <see cref="DateTime" /> values will be in specificity.
        ///   For example, a <paramref name="granularity" /> of <see cref="DateTimeUnit.Day" />
        ///   will only return dates, with all of the time values (hours, minutes, seconds,
        ///   etc.) zeroed out. However, a <paramref name="granularity" /> of
        ///   <see cref="DateTimeUnit.Second" /> will only zero out fractional ticks
        ///   or milliseconds, leaving the hours, minutes, and seconds intact.
        ///   The default is <see cref="DateTimeUnit.Tick" />.
        /// </param>
        public DateTimeGenerator(DateTimeUnit granularity = DateTimeUnit.Tick) :
            this(DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                 DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc),
                 granularity) {}

        /// <summary>
        ///   Instantiates a <see cref="DateTimeGenerator" /> that can create
        ///   <see cref="DateTime" /> values which range from
        ///   <paramref name="low" /> to <see cref="DateTime.MaxValue" />.
        ///   The resultant <see cref="DateTimeGenerator" /> will expect and return
        ///   <see cref="DateTime" /> objects with the <see cref="DateTimeKind" />
        ///   specified on <paramref name="low" />
        /// </summary>
        /// <param name="low">
        ///   Inclusively, the earliest possible <see cref="DateTime" />
        ///   that can be created by this <see cref="DateTimeGenerator" />.
        /// </param>
        /// <param name="granularity">
        ///   How granular generated <see cref="DateTime" /> values will be in specificity.
        ///   For example, a <paramref name="granularity" /> of <see cref="DateTimeUnit.Day" />
        ///   will only return dates, with all of the time values (hours, minutes, seconds,
        ///   etc.) zeroed out. However, a <paramref name="granularity" /> of
        ///   <see cref="DateTimeUnit.Second" /> will only zero out fractional ticks
        ///   or milliseconds, leaving the hours, minutes, and seconds intact.
        ///   The default is <see cref="DateTimeUnit.Tick" />.
        /// </param>
        public DateTimeGenerator(DateTime low, DateTimeUnit granularity = DateTimeUnit.Tick) :
            this(low, DateTime.SpecifyKind(DateTime.MaxValue, low.Kind), granularity) {}

        /// <summary>
        ///   Instantiates a <see cref="DateTimeGenerator" /> that can create
        ///   <see cref="DateTime" /> values which range from
        ///   <paramref name="low" /> to <paramref name="high" />.
        ///   The resultant <see cref="DateTimeGenerator" /> will expect and return
        ///   <see cref="DateTime" /> objects with the <see cref="DateTimeKind" />
        ///   specified on both <paramref name="low" /> and <paramref name="high" />.
        /// </summary>
        /// <param name="low">
        ///   Inclusively, the earliest possible <see cref="DateTime" />
        ///   that can be created by this <see cref="DateTimeGenerator" />.
        /// </param>
        /// <param name="high">
        ///   Exclusively, the latest possible <see cref="DateTime" />
        ///   that can be created by this <see cref="DateTimeGenerator" />.
        /// </param>
        /// <param name="granularity">
        ///   How granular generated <see cref="DateTime" /> values will be in specificity.
        ///   For example, a <paramref name="granularity" /> of <see cref="DateTimeUnit.Day" />
        ///   will only return dates, with all of the time values (hours, minutes, seconds,
        ///   etc.) zeroed out. However, a <paramref name="granularity" /> of
        ///   <see cref="DateTimeUnit.Second" /> will only zero out fractional ticks
        ///   or milliseconds, leaving the hours, minutes, and seconds intact.
        ///   The default is <see cref="DateTimeUnit.Tick" />.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> is later than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="DateTimeKind" /> specified on
        ///   <paramref name="low" /> does not match the <see cref="DateTimeKind" />
        ///   specified on <paramref name="high" />.
        /// </exception>
        public DateTimeGenerator(
            DateTime low,
            DateTime high,
            DateTimeUnit granularity = DateTimeUnit.Tick) {

            if (low.Kind != high.Kind) {
                throw new ArgumentException(
                    $"The {typeof(DateTimeKind).Name} of '{nameof(low)}' ({low.Kind:G}) " +
                    $"did not match the {typeof(DateTimeKind).Name} of '{nameof(high)}' " +
                    $"({high.Kind:G}).",
                    nameof(low)
                );
            }

            if (low.CompareTo(high) >= 0) {
                throw new ArgumentException(
                    $"The {typeof(DateTime).Name} '{nameof(low)}' argument must be " +
                    $"earlier than the {typeof(DateTime).Name} '{nameof(high)}' argument.",
                    nameof(low)
                );
            }

            this.ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);

            var lowByUnit = low.Ticks / this.ticksPerUnit;
            if (low.Ticks % this.ticksPerUnit > 0) {
                lowByUnit += 1L;
            }

            var highByUnit = high.Ticks / this.ticksPerUnit;

            this.unitsGenerator = new Int64Generator(lowByUnit, highByUnit);
            this.EqualityComparer = new KindSensitiveDateTimeEqualityComparer(granularity);
            this.Comparer = new KindSensitiveDateTimeComparer(granularity);

            this.Kind = low.Kind;
            this.Low = new DateTime(this.unitsGenerator.Low * this.ticksPerUnit, this.Kind);
            this.High = new DateTime(this.unitsGenerator.High * this.ticksPerUnit, this.Kind);
        }

        private DateTime NextImpl(Func<Int64> getNextUnits) {
            return new DateTime(getNextUnits() * this.ticksPerUnit, this.Kind);
        }

        private DateTime NextImpl(DateTime other, Func<Int64, Int64> getNextUnits) {
            if (other.Kind != this.Kind) {
                throw new ArgumentException(
                    $"The {typeof(DateTimeKind).Name} of '{nameof(other)}' ({other.Kind:G}) " +
                    $"did not match the {typeof(DateTimeKind).Name} of this " +
                    $"{typeof(DateTimeGenerator).Name} ({this.Kind:G}).",
                    nameof(other)
                );
            }

            return this.NextImpl(
                () => getNextUnits(other.Ticks / this.ticksPerUnit)
            );
        }

        /// <summary>
        ///   Creates a new <see cref="DateTime" /> value that is later than or
        ///   equal to <see cref="Low" /> and earlier than <see cref="High" />.
        ///   The <see cref="DateTimeKind" /> specified on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property.
        /// </summary>
        /// <returns>
        ///   An instance of type <see cref="DateTime" /> that is later than or
        ///   equal to <see cref="Low" /> and earlier than <see cref="High" />.
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property.
        /// </returns>
        public DateTime Next() {
            return this.NextImpl(this.unitsGenerator.Next);
        }

        /// <summary>
        ///   Creates a new <see cref="DateTime" /> value that is later than or
        ///   equal to <see cref="Low" /> and earlier than <see cref="High" />,
        ///   but will not be equal to <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property
        ///   on this <see cref="DateTimeGenerator" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />, nor
        ///   of the same <see cref="DateTimeKind" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="DateTime" /> that is distinct (in terms of ticks or
        ///   <see cref="DateTimeKind" />) from the <see cref="DateTime" /> that
        ///   is returned.
        /// </param>
        /// <returns>
        ///   A <see cref="DateTime" /> that is distinct (in terms of ticks) from
        ///   <paramref name="other" />, but is also later than or equal to
        ///   <see cref="Low" /> and earlier than <see cref="High" />.
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value later than
        ///   or equal to <see cref="Low" /> and earlier than <see cref="High" />.
        ///   This can happen if <see cref="Low" /> and <see cref="High" /> have an
        ///   effective range of one unique quantity of ticks, and the distinct value
        ///   provided via the <paramref name="other" /> parameter has that many ticks.
        /// </exception>
        public DateTime NextDistinct(DateTime other) {
            if (other.Kind != this.Kind) {
                return this.Next();
            }

            return this.NextImpl(other, this.unitsGenerator.NextDistinct);
        }

        /// <summary>
        ///   Creates a new <see cref="DateTime" /> value that is later than
        ///   <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property
        ///   on this <see cref="DateTimeGenerator" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        ///   It does, however, need to have a <see cref="DateTimeKind" /> that
        ///   is consistent with the <see cref="Kind" /> property on this
        ///   <see cref="DateTimeGenerator" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="DateTime" /> that is earlier than the
        ///   <see cref="DateTime" /> that is to be returned (in terms of ticks).
        /// </param>
        /// <returns>
        ///   A <see cref="DateTime" /> that is later than
        ///   <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="Kind" /> defined on this
        ///   <see cref="DateTimeGenerator" /> is different than the
        ///   <see cref="DateTimeKind" /> specified on the <paramref name="other" />
        ///   parameter.
        /// </exception>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value later than
        ///   <paramref name="other" />. This can happen if <see cref="High" />
        ///   is earlier than or equal to <paramref name="other" /> (in terms of ticks).
        /// </exception>
        public DateTime NextGreaterThan(DateTime other) {
            return this.NextImpl(other, this.unitsGenerator.NextGreaterThan);
        }

        /// <summary>
        ///   Creates a new <see cref="DateTime" /> value that is later than
        ///   or equal to <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property
        ///   on this <see cref="DateTimeGenerator" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        ///   It does, however, need to have a <see cref="DateTimeKind" /> that
        ///   is consistent with the <see cref="Kind" /> property on this
        ///   <see cref="DateTimeGenerator" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="DateTime" /> that is earlier than or equal to the
        ///   <see cref="DateTime" /> that is to be returned (in terms of ticks).
        /// </param>
        /// <returns>
        ///   A <see cref="DateTime" /> that is later than or equal to
        ///   <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="Kind" /> defined on this
        ///   <see cref="DateTimeGenerator" /> is different than the
        ///   <see cref="DateTimeKind" /> specified on the <paramref name="other" />
        ///   parameter.
        /// </exception>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value later than or
        ///   equal to <paramref name="other" />. This can happen if <see cref="High" />
        ///   is earlier than or equal to <paramref name="other" /> (in terms of ticks).
        /// </exception>
        public DateTime NextGreaterThanOrEqualTo(DateTime other) {
            return this.NextImpl(other, this.unitsGenerator.NextGreaterThanOrEqualTo);
        }

        /// <summary>
        ///   Creates a new <see cref="DateTime" /> value that is earlier than
        ///   <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property
        ///   on this <see cref="DateTimeGenerator" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        ///   It does, however, need to have a <see cref="DateTimeKind" /> that
        ///   is consistent with the <see cref="Kind" /> property on this
        ///   <see cref="DateTimeGenerator" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="DateTime" /> that is later than the
        ///   <see cref="DateTime" /> that is to be returned (in terms of ticks).
        /// </param>
        /// <returns>
        ///   A <see cref="DateTime" /> that is earlier than
        ///   <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="Kind" /> defined on this
        ///   <see cref="DateTimeGenerator" /> is different than the
        ///   <see cref="DateTimeKind" /> specified on the <paramref name="other" />
        ///   parameter.
        /// </exception>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value earlier than
        ///   <paramref name="other" />. This can happen if <see cref="Low" />
        ///   is later than or equal to <paramref name="other" /> (in terms of ticks).
        /// </exception>
        public DateTime NextLessThan(DateTime other) {
            return this.NextImpl(other, this.unitsGenerator.NextLessThan);
        }

        /// <summary>
        ///   Creates a new <see cref="DateTime" /> value that is earlier than or
        ///   equal to <paramref name="other" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property
        ///   on this <see cref="DateTimeGenerator" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        ///   It does, however, need to have a <see cref="DateTimeKind" /> that
        ///   is consistent with the <see cref="Kind" /> property on this
        ///   <see cref="DateTimeGenerator" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="DateTime" /> that is later than or equal to
        ///   the <see cref="DateTime" /> that is to be returned (in terms of ticks).
        /// </param>
        /// <returns>
        ///   A <see cref="DateTime" /> that is earlier than or equal to both
        ///   <paramref name="other" /> and <see cref="High" /> (in terms of ticks).
        ///   The <see cref="DateTimeKind" /> on the <see cref="DateTime" />
        ///   returned will be equal to the <see cref="Kind" /> property.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   Thrown when the <see cref="Kind" /> defined on this
        ///   <see cref="DateTimeGenerator" /> is different than the
        ///   <see cref="DateTimeKind" /> specified on the <paramref name="other" />
        ///   parameter.
        /// </exception>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value earlier than
        ///   or equal to <paramref name="other" />. This can happen if <see cref="Low" />
        ///   is later than <paramref name="other" /> (in terms of ticks).
        /// </exception>
        public DateTime NextLessThanOrEqualTo(DateTime other) {
            return this.NextImpl(other, this.unitsGenerator.NextLessThanOrEqualTo);
        }

    }

}
