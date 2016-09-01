using System;

namespace Peddler {

    /// <summary>
    ///   A generator for numbers of type <see cref="T:System.Byte" />. Depending on the
    ///   constructor used, the generator can either utilize a range of all possible
    ///   <see cref="T:System.Byte" /> values or a constrained, smaller range.
    /// </summary>
    /// <remarks>
    ///   The range of possible <see cref="T:System.Byte" /> values will always be contiguous.
    /// </remarks>
    public class ByteGenerator : IntegralGenerator<Byte> {

        private Random random { get; } = new Random();

        /// <summary>
        ///   Instantiates an <see cref="T:ByteGenerator" /> that can create
        ///   <see cref="T:System.Byte" /> values that range from 0 (inclusively) to
        ///   <see cref="M:System.Byte.MaxValue" /> (exclusively).
        /// </summary>
        public ByteGenerator() :
            this(0, Byte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:ByteGenerator" /> that can create
        ///   <see cref="T:System.Byte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <see cref="M:System.Byte.MaxValue" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.Byte" /> boundary for this generator.
        /// </param>
        public ByteGenerator(Byte low) :
            this(low, Byte.MaxValue) {}

        /// <summary>
        ///   Instantiates an <see cref="T:Peddler.ByteGenerator" /> that can create
        ///   <see cref="T:System.Byte" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="T:System.Byte" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <see cref="T:System.Byte" /> boundary for this generator.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when <paramref name="low" /> is less than <paramref name="high" />.
        /// </exception>
        public ByteGenerator(Byte low, Byte high) :
            base(low, high) {}

        protected override sealed Byte Next(Byte low, Byte high) {
            return this.random.NextByte(low, high);
        }

        protected override sealed Byte SubtractOne(Byte value) {
            return (Byte)(value - 1);
        }

        protected override sealed Byte AddOne(Byte value) {
            return (Byte)(value + 1);
        }

    }

}
