using System;

namespace Peddler {

    public class UInt16Generator : IntegralGenerator<UInt16> {

        private Random random { get; } = new Random();

        public UInt16Generator() :
            this(0, UInt16.MaxValue) {}

        public UInt16Generator(UInt16 low) :
            this(low, UInt16.MaxValue) {}

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
