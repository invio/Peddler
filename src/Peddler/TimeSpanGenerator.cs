using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   A generator for values of type <see cref="TimeSpan" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="TimeSpan" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="TimeSpan" /> values will always be contiguous.
    ///   Each <see cref="TimeSpan" /> is considered distinct from another by
    ///   the number of ticks used to represent its value.
    /// </remarks>
    public class TimeSpanGenerator : IIntegralGenerator<TimeSpan> {

        private static EqualityComparer<TimeSpan> defaultEqualityComparer { get; }
        private static Comparer<TimeSpan> defaultComparer { get; }

        static TimeSpanGenerator() {
            defaultEqualityComparer = EqualityComparer<TimeSpan>.Default;
            defaultComparer = Comparer<TimeSpan>.Default;
        }

        private Int64Generator tickGenerator { get; }

        /// <summary>
        ///   Inclusively, the minimum possible <see cref="TimeSpan" />
        ///   that can be created by this <see cref="TimeSpanGenerator" />.
        /// </summary>
        /// <remarks>
        ///   As a <see cref="TimeSpan" /> is represented by a number of ticks,
        ///   this can be negative.
        /// </remarks>
        public TimeSpan Low { get; }

        /// <summary>
        ///   Exclusively, the maximum possible <see cref="TimeSpan" />
        ///   that can be created by this <see cref="TimeSpanGenerator" />.
        /// </summary>
        /// <remarks>
        ///   As a <see cref="TimeSpan" /> is represented by a number of ticks,
        ///   this can be negative.
        /// </remarks>
        public TimeSpan High { get; }

        /// <summary>
        ///   The comparer used to determine if two <see cref="TimeSpan" />
        ///   instances are equal in value.
        /// </summary>
        public IEqualityComparer<TimeSpan> EqualityComparer { get; } = defaultEqualityComparer;

        /// <summary>
        ///   The comparison used to determine if one <see cref="TimeSpan" />
        ///   instance is earlier than, the same time as, or later than another
        ///   <see cref="TimeSpan" /> instance.
        /// </summary>
        public IComparer<TimeSpan> Comparer { get; } = defaultComparer;

        /// <summary>
        ///   Instantiates a <see cref="TimeSpanGenerator" /> that can create
        ///   <see cref="TimeSpan" /> values which range in ticks from
        ///   <see cref="TimeSpan.Zero" /> to <see cref="TimeSpan.MaxValue" />.
        /// </summary>
        public TimeSpanGenerator() :
            this(TimeSpan.Zero, TimeSpan.MaxValue) {}

        /// <summary>
        ///   Instantiates a <see cref="TimeSpanGenerator" /> that can create
        ///   <see cref="TimeSpan" /> values which range in ticks from
        ///   <paramref name="low" /> to <see cref="TimeSpan.MaxValue" />.
        /// </summary>
        /// <param name="low">
        ///   A <see cref="TimeSpan" /> that represents, inclusively, the smallest
        ///   number of ticks a <see cref="TimeSpan" /> can have that is created
        ///   by this <see cref="DateTimeGenerator" />.
        /// </param>
        public TimeSpanGenerator(TimeSpan low) :
            this(low, TimeSpan.MaxValue) {}

        /// <summary>
        ///   Instantiates a <see cref="TimeSpanGenerator" /> that can create
        ///   <see cref="TimeSpan" /> values which range in ticks from
        ///   <paramref name="low" /> to <paramref name="high" />.
        /// </summary>
        /// <param name="low">
        ///   A <see cref="TimeSpan" /> that represents, inclusively, the smallest
        ///   number of ticks a <see cref="TimeSpan" /> can have that is created
        ///   by this <see cref="DateTimeGenerator" />.
        /// </param>
        /// <param name="high">
        ///   A <see cref="TimeSpan" /> that represents, exclusively, the greatest
        ///   number of ticks a <see cref="TimeSpan" /> can have that is created
        ///   by this <see cref="DateTimeGenerator" />.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> has fewer ticks than
        ///   <paramref name="high" />.
        /// </exception>
        public TimeSpanGenerator(TimeSpan low, TimeSpan high) {
            if (low.CompareTo(high) >= 0) {
                throw new ArgumentException(
                    $"The {typeof(TimeSpan).Name} '{nameof(low)}' argument must have " +
                    $"fewer ticks than the {typeof(TimeSpan).Name} '{nameof(high)}' argument."
                );
            }

            this.tickGenerator = new Int64Generator(low.Ticks, high.Ticks);
            this.Low = low;
            this.High = high;
        }

        private TimeSpan NextImpl(Func<Int64> getNextTicks) {
            return new TimeSpan(getNextTicks());
        }

        private TimeSpan NextImpl(TimeSpan other, Func<Int64, Int64> getNextTicks) {
            return this.NextImpl(() => getNextTicks(other.Ticks));
        }

        /// <summary>
        ///   Creates a new <see cref="TimeSpan" /> value that has a greater
        ///   than or equal to number of ticks than <see cref="Low" />, but a
        ///   fewer number of ticks than <see cref="High" />.
        /// </summary>
        /// <returns>
        ///   An instance of type <see cref="TimeSpan" /> that has a greater
        ///   than or equal to number of ticks than <see cref="Low" />, but a
        ///   fewer number of ticks than <see cref="High" />.
        /// </returns>
        public TimeSpan Next() {
            return this.NextImpl(this.tickGenerator.Next);
        }

        /// <summary>
        ///   Creates a new <see cref="TimeSpan" /> value that has a greater than
        ///   or equal to number of ticks than  <see cref="Low" />, a fewer number
        ///   of ticks than <see cref="High" />, but an different number of ticks
        ///   than <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="TimeSpan" /> that is distinct (in terms of ticks)
        ///   from the <see cref="TimeSpan" /> that is to be returned.
        /// </param>
        /// <returns>
        ///   A <see cref="TimeSpan" /> that is distinct (in terms of ticks) from
        ///   <paramref name="other" />, but also has a greater than or equal number
        ///   of ticks than <see cref="Low" />, and fewer ticks than <see cref="High" />.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value has a greater than
        ///   or equal number of ticks than <see cref="Low" /> and fewer ticks than
        ///   <see cref="High" />. This can happen if <see cref="Low" /> and
        ///   <see cref="High" /> have an effective range of one unique quantity of ticks,
        ///   and the distinct value provided via the <paramref name="other" /> parameter
        ///   has that many ticks.
        /// </exception>
        public TimeSpan NextDistinct(TimeSpan other) {
            return this.NextImpl(other, this.tickGenerator.NextDistinct);
        }

        /// <summary>
        ///   Creates a new <see cref="TimeSpan" /> that will have a greater
        ///   number of ticks than <paramref name="other" />, but be between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="TimeSpan" /> that has a number of ticks that is
        ///   less than the <see cref="TimeSpan" /> that will be generated.
        /// </param>
        /// <returns>
        ///   A <see cref="TimeSpan" /> that is greater than both
        ///   <paramref name="other" /> and <see cref="Low" /> in terms of ticks.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a <see cref="TimeSpan" />
        ///   that is greater than <paramref name="other" /> and <see cref="Low" />
        ///   in terms of ticks. This can happen if <see cref="High" /> has fewer ticks
        ///   than <paramref name="other" />.
        /// </exception>
        public TimeSpan NextGreaterThan(TimeSpan other) {
            return this.NextImpl(other, this.tickGenerator.NextGreaterThan);
        }

        /// <summary>
        ///   Creates a new <see cref="TimeSpan" /> that will have a greater or equal
        ///   number of ticks than <paramref name="other" />, but be between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="TimeSpan" /> that has a number of ticks that is
        ///   less than or equal to the <see cref="TimeSpan" /> that will be generated.
        /// </param>
        /// <returns>
        ///   A <see cref="TimeSpan" /> that is greater than or equal to both
        ///   <paramref name="other" /> and <see cref="Low" /> in terms of ticks.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a <see cref="TimeSpan" />
        ///   that is greater than or equal to <paramref name="other" /> and <see cref="Low" />
        ///   in terms of ticks. This can happen if <see cref="High" /> has fewer ticks
        ///   than <paramref name="other" />.
        /// </exception>
        public TimeSpan NextGreaterThanOrEqualTo(TimeSpan other) {
            return this.NextImpl(other, this.tickGenerator.NextGreaterThanOrEqualTo);
        }

        /// <summary>
        ///   Creates a new <see cref="TimeSpan" /> that will have a fewer
        ///   number of ticks than <paramref name="other" />, but be between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="TimeSpan" /> that has a number of ticks that is
        ///   greater than the <see cref="TimeSpan" /> that will be generated.
        /// </param>
        /// <returns>
        ///   A <see cref="TimeSpan" /> that is less than both
        ///   <paramref name="other" /> and <see cref="High" /> in terms of ticks.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a <see cref="TimeSpan" />
        ///   that is less than <paramref name="other" /> and <see cref="High" />
        ///   in terms of ticks. This can happen if <see cref="Low" /> has fewer ticks than
        ///   <paramref name="other" />.
        /// </exception>
        public TimeSpan NextLessThan(TimeSpan other) {
            return this.NextImpl(other, this.tickGenerator.NextLessThan);
        }

        /// <summary>
        ///   Creates a new <see cref="TimeSpan" /> that will have a fewer or equal
        ///   number of ticks than <paramref name="other" />, but be between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A <see cref="TimeSpan" /> that has a number of ticks that is greater
        ///   than or equal to the <see cref="TimeSpan" /> that will be generated.
        /// </param>
        /// <returns>
        ///   A <see cref="TimeSpan" /> that is less than or equal to both
        ///   <paramref name="other" /> and <see cref="High" /> in terms of ticks.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a <see cref="TimeSpan" />
        ///   that is less than or equal to <paramref name="other" /> and <see cref="High" />
        ///   in terms of ticks. This can happen if <see cref="Low" /> has fewer ticks than
        ///   <paramref name="other" />.
        /// </exception>
        public TimeSpan NextLessThanOrEqualTo(TimeSpan other) {
            return this.NextImpl(other, this.tickGenerator.NextLessThanOrEqualTo);
        }

    }

}
