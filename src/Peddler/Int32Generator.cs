using System;

namespace Peddler {

    public class Int32Generator : IntegralGenerator<Int32> {

        private Random random { get; } = new Random();

        public Int32Generator() :
            this(0, Int32.MaxValue) {}

        public Int32Generator(Int32 low) :
            this(low, Int32.MaxValue) {}

        public Int32Generator(Int32 low, Int32 high) :
            base(low, high) {}

        protected override sealed Int32 Next(Int32 low, Int32 high) {
            return this.random.Next(low, high);
        }

        protected override sealed Int32 SubtractOne(Int32 value) {
            return value - 1;
        }

        protected override sealed Int32 AddOne(Int32 value) {
            return value + 1;
        }

    }

}
