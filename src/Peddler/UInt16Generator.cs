using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="T:System.UInt16" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="T:System.UInt16" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="T:System.UInt16" /> values will always be contiguous.
    /// </remarks>
    public class UInt16Generator : IntegralGenerator<UInt16> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="T:UInt16Generator" /> that can create
        ///   <see cref="T:System.UInt16" /> values that range from 0 (inclusively) to
        ///   <see cref="M:System.UInt16.MaxValue" /> (exclusively).
        /// </summary>
        public UInt16Generator() :
            this(0, UInt16.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:UInt16Generator" /> that can create
        ///   <see cref="T:System.UInt16" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="M:System.UInt16.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.UInt16" /> boundary for this generator.
        /// </param>
        public UInt16Generator(UInt16 low) :
            this(low, UInt16.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:Peddler.UInt16Generator" /> that can create
        ///   <see cref="T:System.UInt16" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.UInt16" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="T:System.UInt16" /> boundary for this generator.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when <paramref name="low" /> is less than <paramref name="high" />.
        /// </exception>
        public UInt16Generator(UInt16 low, UInt16 high) :
            base(low, high) {}

        protected override sealed UInt16 Next(UInt16 low, UInt16 high) {
            return this.random.NextUInt16(low, high);
        }

        protected override sealed UInt16 SubtractOne(UInt16 value) {
            return (UInt16)(value - 1);
        }

        protected override sealed UInt16 AddOne(UInt16 value) {
            return (UInt16)(value + 1);
        }

    }

}
