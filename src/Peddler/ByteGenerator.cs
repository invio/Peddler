using System;

namespace Peddler {

    public class ByteGenerator : IntegralGenerator<Byte> {

        private Random random { get; } = new Random();

        public ByteGenerator() :
            this(0, Byte.MaxValue) {}

        public ByteGenerator(Byte low) :
            this(low, Byte.MaxValue) {}

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
