using System;
using System.Threading;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="UInt32" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="UInt32" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="UInt32" /> values will always be contiguous.
    /// </remarks>
    public class UInt32Generator : IntegralGenerator<UInt32> {

        private static ThreadLocal<Random> random { get; } =
            new ThreadLocal<Random>(() => new Random());

        /// <summary>
        ///   Instantiates an <see cref="UInt32Generator" /> that can create
        ///   <see cref="UInt32" /> values that range from 0 (inclusively) to
        ///   <see cref="UInt32.MaxValue" /> (exclusively).
        /// </summary>
        public UInt32Generator() :
            this(0, UInt32.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="UInt32Generator" /> that can create
        ///   <see cref="UInt32" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="UInt32.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="UInt32" /> boundary for this generator.
        /// </param>
        public UInt32Generator(UInt32 low) :
            this(low, UInt32.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="UInt32Generator" /> that can create
        ///   <see cref="UInt32" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="UInt32" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="UInt32" /> boundary for this generator.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> is greater than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        public UInt32Generator(UInt32 low, UInt32 high) :
            base(low, high) {}

        /// <inheritdoc />
        protected override sealed UInt32 Next(UInt32 low, UInt32 high) {
            return random.Value.NextUInt32(low, high);
        }

        /// <inheritdoc />
        protected override sealed UInt32 SubtractOne(UInt32 value) {
            return value - 1;
        }

        /// <inheritdoc />
        protected override sealed UInt32 AddOne(UInt32 value) {
            return value + 1;
        }

    }

}
