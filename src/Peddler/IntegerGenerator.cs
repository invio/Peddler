using System;

namespace Peddler {

    public class IntegerGenerator : IDistinctGenerator<int> {

        private Random random { get; } = new Random();
        private int low { get; }
        private int high { get; }

        public IntegerGenerator() :
            this(0, Int32.MaxValue) {}

        public IntegerGenerator(int low) :
            this(low, Int32.MaxValue) {}

        public IntegerGenerator(int low, int high) {
            if (this.low < this.high) {
                throw new ArgumentException(
                    $"The '{nameof(low)}' argument ({low:N0}) cannot be greater " +
                    $"than the '{nameof(high)}' argument ({high:N0}).",
                    nameof(low)
                );
            }

            this.low = low;
            this.high = high;
        }

        public int Next() {
            return this.random.Next(this.low, this.high);
        }

        public int NextDistinct(int other) {
            if (other < this.low || other >= this.high) {
                return this.Next();
            }

            if (this.low == this.high) {
                throw new InvalidOperationException(
                    $"The '{nameof(low)}' boundary and '{nameof(high)}' boundary " +
                    $"are both '{low:N0}', so a distinct value cannot be generated."
                );
            }

            // Remove 1 value to represent the range of values minus a single
            // value to represent the one given by the caller as 'other'.
            // If we get a value equal to or greater than 'other', add one.

            // This is better with an example to explain why ...

            // Given the following values:
            //   low = 1
            //   high = 6
            //   other = 3
            //
            // Because the 'high' is an exclusive boundary, the integers we can generate
            // are within the range of [ 1, 2, 3, 4, 5 ]. We remove one to represent
            // the 'other' value, giving us a range of [ 1, 2, 3, 4 ]. One can see that
            // this includes our current value of '3'. However, if we add a one to the
            // result if it is equal to or greater to '3', our range becomes [ 1, 2, 4, 5 ].
            // This gives us a perfectly balanced range of possibilities.

            var nextValue = this.random.Next(this.low, this.high - 1);

            if (nextValue >= other) {
                nextValue++;
            }

            return nextValue;
        }

    }

}
