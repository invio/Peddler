using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="SByte" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="SByte" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="SByte" /> values will always be contiguous.
    /// </remarks>
    public class SByteGenerator : IntegralGenerator<SByte> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="SByteGenerator" /> that can create
        ///   <see cref="SByte" /> values that range from 0 (inclusively) to
        ///   <see cref="SByte.MaxValue" /> (exclusively).
        /// </summary>
        public SByteGenerator() :
            this(0, SByte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="SByteGenerator" /> that can create
        ///   <see cref="SByte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="SByte.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="SByte" /> boundary for this generator.
        /// </param>
        public SByteGenerator(SByte low) :
            this(low, SByte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="SByteGenerator" /> that can create
        ///   <see cref="SByte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="SByte" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="SByte" /> boundary for this generator.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> is greater than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        public SByteGenerator(SByte low, SByte high) :
            base(low, high) {}

        /// <inheritdoc />
        protected override sealed SByte Next(SByte low, SByte high) {
            return this.random.NextSByte(low, high);
        }

        /// <inheritdoc />
        protected override sealed SByte SubtractOne(SByte value) {
            return (SByte)(value - 1);
        }

        /// <inheritdoc />
        protected override sealed SByte AddOne(SByte value) {
            return (SByte)(value + 1);
        }

    }

}
