using System;
using Xunit;

namespace Peddler {

    public class BooleanGeneratorTests {

        private const int numberOfAttempts = 1000;

        [Fact]
        public void Next() {
            var generator = new BooleanGenerator();

            bool hasTrue = false;
            bool hasFalse = false;

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                if (value) {
                    hasTrue = true;
                } else {
                    hasFalse = true;
                }

                if (hasTrue && hasFalse) {
                    break;
                }
            }

            Assert.True(
                hasTrue,
                $"Expected at least instance of 'true' to " +
                $"be generated over {numberOfAttempts:N0} attempts."
            );

            Assert.True(
                hasFalse,
                $"Expected at least instance of 'false' to " +
                $"be generated over {numberOfAttempts:N0} attempts."
            );
        }

        [Fact]
        public void NextDistinct() {
            var generator = new BooleanGenerator();
            var previousValue = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(previousValue);

                Assert.NotEqual(previousValue, value);
                Assert.False(generator.EqualityComparer.Equals(previousValue, value));

                previousValue = value;
            }
        }

    }

}
