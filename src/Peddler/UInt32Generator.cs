using System;

namespace Peddler {

    public class UInt32Generator : IntegralGenerator<UInt32> {

        private Random random { get; } = new Random();

        public UInt32Generator() :
            this(0, UInt32.MaxValue) {}

        public UInt32Generator(UInt32 low) :
            this(low, UInt32.MaxValue) {}

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
