using System;

namespace Peddler {

    public class Int64Generator : IntegralGenerator<Int64> {

        private Random random { get; } = new Random();

        public Int64Generator() :
            this(0, Int64.MaxValue) {}

        public Int64Generator(Int64 low) :
            this(low, Int64.MaxValue) {}

        public Int64Generator(Int64 low, Int64 high) :
            base(low, high) {}

        protected override sealed Int64 Next(Int64 low, Int64 high) {
            return this.random.NextInt64(low, high);
        }

        protected override sealed Int64 SubtractOne(Int64 value) {
            return value - 1;
        }

        protected override sealed Int64 AddOne(Int64 value) {
            return value + 1;
        }

    }

}
