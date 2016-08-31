using System;

namespace Peddler {

    public class UInt64Generator : IntegralGenerator<UInt64> {

        private Random random { get; } = new Random();

        public UInt64Generator() :
            this(0, UInt64.MaxValue) {}

        public UInt64Generator(UInt64 low) :
            this(low, UInt64.MaxValue) {}

        public UInt64Generator(UInt64 low, UInt64 high) :
            base(low, high) {}

        protected override sealed UInt64 Next(UInt64 low, UInt64 high) {
            return this.random.NextUInt64(low, high);
        }

        protected override sealed UInt64 SubtractOne(UInt64 value) {
            return value - 1;
        }

        protected override sealed UInt64 AddOne(UInt64 value) {
            return value + 1;
        }

    }

}
