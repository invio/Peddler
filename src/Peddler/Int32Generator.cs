using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="Int32" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="Int32" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="Int32" /> values will always be contiguous.
    /// </remarks>
    public class Int32Generator : IntegralGenerator<Int32> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="Int32Generator" /> that can create
        ///   <see cref="Int32" /> values that range from 0 (inclusively) to
        ///   <see cref="Int32.MaxValue" /> (exclusively).
        /// </summary>
        public Int32Generator() :
            this(0, Int32.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="Int32Generator" /> that can create
        ///   <see cref="Int32" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="Int32.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Int32" /> boundary for this generator.
        /// </param>
        public Int32Generator(Int32 low) :
            this(low, Int32.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="Int32Generator" /> that can create
        ///   <see cref="Int32" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Int32" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="Int32" /> boundary for this generator.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> is greater than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        public Int32Generator(Int32 low, Int32 high) :
            base(low, high) {}

        /// <inheritdoc />
        protected override sealed Int32 Next(Int32 low, Int32 high) {
            return this.random.Next(low, high);
        }

        /// <inheritdoc />
        protected override sealed Int32 SubtractOne(Int32 value) {
            return value - 1;
        }

        /// <inheritdoc />
        protected override sealed Int32 AddOne(Int32 value) {
            return value + 1;
        }

    }

}
