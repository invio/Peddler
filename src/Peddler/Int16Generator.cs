using System;

namespace Peddler {

    public class Int16Generator : IntegralGenerator<Int16> {

        private Random random { get; } = new Random();

        public Int16Generator() :
            this(0, Int16.MaxValue) {}

        public Int16Generator(Int16 low) :
            this(low, Int16.MaxValue) {}

        public Int16Generator(Int16 low, Int16 high) :
            base(low, high) {}

        protected override sealed Int16 Next(Int16 low, Int16 high) {
            return this.random.NextInt16(low, high);
        }

        protected override sealed Int16 SubtractOne(Int16 value) {
            return (Int16)(value - 1);
        }

        protected override sealed Int16 AddOne(Int16 value) {
            return (Int16)(value + 1);
        }

    }

}
