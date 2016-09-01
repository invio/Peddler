using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="T:System.UInt32" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="T:System.UInt32" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="T:System.UInt32" /> values will always be contiguous.
    /// </remarks>
    public class UInt32Generator : IntegralGenerator<UInt32> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="T:UInt32Generator" /> that can create
        ///   <see cref="T:System.UInt32" /> values that range from 0 (inclusively) to
        ///   <see cref="M:System.UInt32.MaxValue" /> (exclusively).
        /// </summary>
        public UInt32Generator() :
            this(0, UInt32.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:UInt32Generator" /> that can create
        ///   <see cref="T:System.UInt32" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="M:System.UInt32.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.UInt32" /> boundary for this generator.
        /// </param>
        public UInt32Generator(UInt32 low) :
            this(low, UInt32.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:Peddler.UInt32Generator" /> that can create
        ///   <see cref="T:System.UInt32" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.UInt32" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="T:System.UInt32" /> boundary for this generator.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when <paramref name="low" /> is less than <paramref name="high" />.
        /// </exception>
        public UInt32Generator(UInt32 low, UInt32 high) :
            base(low, high) {}

        protected override sealed UInt32 Next(UInt32 low, UInt32 high) {
            return this.random.NextUInt32(low, high);
        }

        protected override sealed UInt32 SubtractOne(UInt32 value) {
            return value - 1;
        }

        protected override sealed UInt32 AddOne(UInt32 value) {
            return value + 1;
        }

    }

}
