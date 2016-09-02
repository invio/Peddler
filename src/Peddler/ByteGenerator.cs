using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="Byte" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="Byte" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="Byte" /> values will always be contiguous.
    /// </remarks>
    public class ByteGenerator : IntegralGenerator<Byte> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="ByteGenerator" /> that can create
        ///   <see cref="Byte" /> values that range from 0 (inclusively) to
        ///   <see cref="Byte.MaxValue" /> (exclusively).
        /// </summary>
        public ByteGenerator() :
            this(0, Byte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="ByteGenerator" /> that can create
        ///   <see cref="Byte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="Byte.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Byte" /> boundary for this generator.
        /// </param>
        public ByteGenerator(Byte low) :
            this(low, Byte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="ByteGenerator" /> that can create
        ///   <see cref="Byte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Byte" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="Byte" /> boundary for this generator.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="low" /> is greater than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        public ByteGenerator(Byte low, Byte high) :
            base(low, high) {}

        /// <inheritdoc />
        protected override sealed Byte Next(Byte low, Byte high) {
            return this.random.NextByte(low, high);
        }

        /// <inheritdoc />
        protected override sealed Byte SubtractOne(Byte value) {
            return (Byte)(value - 1);
        }

        /// <inheritdoc />
        protected override sealed Byte AddOne(Byte value) {
            return (Byte)(value + 1);
        }

    }

}
