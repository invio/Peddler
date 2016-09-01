using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="T:System.SByte" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="T:System.SByte" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="T:System.SByte" /> values will always be contiguous.
    /// </remarks>
    public class SByteGenerator : IntegralGenerator<SByte> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="T:SByteGenerator" /> that can create
        ///   <see cref="T:System.SByte" /> values that range from 0 (inclusively) to
        ///   <see cref="M:System.SByte.MaxValue" /> (exclusively).
        /// </summary>
        public SByteGenerator() :
            this(0, SByte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:SByteGenerator" /> that can create
        ///   <see cref="T:System.SByte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="M:System.SByte.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.SByte" /> boundary for this generator.
        /// </param>
        public SByteGenerator(SByte low) :
            this(low, SByte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:Peddler.SByteGenerator" /> that can create
        ///   <see cref="T:System.SByte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.SByte" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="T:System.SByte" /> boundary for this generator.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when <paramref name="low" /> is less than <paramref name="high" />.
        /// </exception>
        public SByteGenerator(SByte low, SByte high) :
            base(low, high) {}

        protected override sealed SByte Next(SByte low, SByte high) {
            return this.random.NextSByte(low, high);
        }

        protected override sealed SByte SubtractOne(SByte value) {
            return (SByte)(value - 1);
        }

        protected override sealed SByte AddOne(SByte value) {
            return (SByte)(value + 1);
        }

    }

}
