using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="T:System.Int64" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="T:System.Int64" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="T:System.Int64" /> values will always be contiguous.
    /// </remarks>
    public class Int64Generator : IntegralGenerator<Int64> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="T:Int64Generator" /> that can create
        ///   <see cref="T:System.Int64" /> values that range from 0 (inclusively) to
        ///   <see cref="M:System.Int64.MaxValue" /> (exclusively).
        /// </summary>
        public Int64Generator() :
            this(0, Int64.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:Int64Generator" /> that can create
        ///   <see cref="T:System.Int64" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="M:System.Int64.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.Int64" /> boundary for this generator.
        /// </param>
        public Int64Generator(Int64 low) :
            this(low, Int64.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:Peddler.Int64Generator" /> that can create
        ///   <see cref="T:System.Int64" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.Int64" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="T:System.Int64" /> boundary for this generator.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when <paramref name="low" /> is less than <paramref name="high" />.
        /// </exception>
        public Int64Generator(Int64 low, Int64 high) :
            base(low, high) {}

        protected override sealed Int64 Next(Int64 low, Int64 high) {
            return this.random.NextInt64(low, high);
        }

        protected override sealed Int64 SubtractOne(Int64 value) {
            return value - 1;
        }

        protected override sealed Int64 AddOne(Int64 value) {
            return value + 1;
        }

    }

}
