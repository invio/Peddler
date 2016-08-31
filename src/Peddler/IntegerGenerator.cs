using System;

namespace Peddler {

    public class IntegerGenerator : IntegralGenerator<int> {

        private Random random { get; } = new Random();

        public IntegerGenerator() :
            this(0, Int32.MaxValue) {}

        public IntegerGenerator(int low) :
            this(low, Int32.MaxValue) {}

        public IntegerGenerator(int low, int high) :
            base(low, high) {}

        protected override sealed int Next(int low, int high) {
            return this.random.Next(low, high);
        }

        protected override sealed int SubtractOne(int value) {
            return --value;
        }

        protected override sealed int AddOne(int value) {
            return ++value;
        }

    }

}
