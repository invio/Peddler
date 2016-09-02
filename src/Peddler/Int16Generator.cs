using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="Int16" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="Int16" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="Int16" /> values will always be contiguous.
    /// </remarks>
    public class Int16Generator : IntegralGenerator<Int16> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="Int16Generator" /> that can create
        ///   <see cref="Int16" /> values that range from 0 (inclusively) to
        ///   <see cref="Int16.MaxValue" /> (exclusively).
        /// </summary>
        public Int16Generator() :
            this(0, Int16.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="Int16Generator" /> that can create
        ///   <see cref="Int16" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="Int16.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Int16" /> boundary for this generator.
        /// </param>
        public Int16Generator(Int16 low) :
            this(low, Int16.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="Int16Generator" /> that can create
        ///   <see cref="Int16" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Int16" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="Int16" /> boundary for this generator.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> is greater than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        public Int16Generator(Int16 low, Int16 high) :
            base(low, high) {}

        /// <inheritdoc />
        protected override sealed Int16 Next(Int16 low, Int16 high) {
            return this.random.NextInt16(low, high);
        }

        /// <inheritdoc />
        protected override sealed Int16 SubtractOne(Int16 value) {
            return (Int16)(value - 1);
        }

        /// <inheritdoc />
        protected override sealed Int16 AddOne(Int16 value) {
            return (Int16)(value + 1);
        }

    }

}
