using System;

namespace Peddler {

    public class SByteGenerator : IntegralGenerator<SByte> {

        private Random random { get; } = new Random();

        public SByteGenerator() :
            this(0, SByte.MaxValue) {}

        public SByteGenerator(SByte low) :
            this(low, SByte.MaxValue) {}

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
