using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="T:System.UInt64" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="T:System.UInt64" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="T:System.UInt64" /> values will always be contiguous.
    /// </remarks>
    public class UInt64Generator : IntegralGenerator<UInt64> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="T:UInt64Generator" /> that can create
        ///   <see cref="T:System.UInt64" /> values that range from 0 (inclusively) to
        ///   <see cref="M:System.UInt64.MaxValue" /> (exclusively).
        /// </summary>
        public UInt64Generator() :
            this(0, UInt64.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:UInt64Generator" /> that can create
        ///   <see cref="T:System.UInt64" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="M:System.UInt64.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.UInt64" /> boundary for this generator.
        /// </param>
        public UInt64Generator(UInt64 low) :
            this(low, UInt64.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:Peddler.UInt64Generator" /> that can create
        ///   <see cref="T:System.UInt64" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.UInt64" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="T:System.UInt64" /> boundary for this generator.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when <paramref name="low" /> is less than <paramref name="high" />.
        /// </exception>
        public UInt64Generator(UInt64 low, UInt64 high) :
            base(low, high) {}

        /// <inheritdoc />
        protected override sealed UInt64 Next(UInt64 low, UInt64 high) {
            return this.random.NextUInt64(low, high);
        }

        /// <inheritdoc />
        protected override sealed UInt64 SubtractOne(UInt64 value) {
            return value - 1;
        }

        /// <inheritdoc />
        protected override sealed UInt64 AddOne(UInt64 value) {
            return value + 1;
        }

    }

}
