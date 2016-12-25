using System;
using System.Threading;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="Int64" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="Int64" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="Int64" /> values will always be contiguous.
    /// </remarks>
    public class Int64Generator : IntegralGenerator<Int64> {

        private static ThreadLocal<Random> random { get; } =
            new ThreadLocal<Random>(() => new Random());

        /// <summary>
        ///   Instantiates an <see cref="Int64Generator" /> that can create
        ///   <see cref="Int64" /> values that range from 0 (inclusively) to
        ///   <see cref="Int64.MaxValue" /> (exclusively).
        /// </summary>
        public Int64Generator() :
            this(0, Int64.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="Int64Generator" /> that can create
        ///   <see cref="Int64" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="Int64.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Int64" /> boundary for this generator.
        /// </param>
        public Int64Generator(Int64 low) :
            this(low, Int64.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="Int64Generator" /> that can create
        ///   <see cref="Int64" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Int64" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="Int64" /> boundary for this generator.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> is greater than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        public Int64Generator(Int64 low, Int64 high) :
            base(low, high) {}

        /// <inheritdoc />
        protected override sealed Int64 Next(Int64 low, Int64 high) {
            return random.Value.NextInt64(low, high);
        }

        /// <inheritdoc />
        protected override sealed Int64 SubtractOne(Int64 value) {
            return value - 1;
        }

        /// <inheritdoc />
        protected override sealed Int64 AddOne(Int64 value) {
            return value + 1;
        }

    }

}
