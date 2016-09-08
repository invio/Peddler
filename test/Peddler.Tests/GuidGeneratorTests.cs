using System;
using Xunit;

namespace Peddler {

    public class GuidGeneratorTests {

        private const int NUMBER_OF_ATTEMPTS = 100;

        [Fact]
        public void Next_NonEmptyGuids() {
            var generator = new GuidGenerator();

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                Assert.NotEqual(Guid.Empty, generator.Next());
            }

        }

        [Fact]
        public void NextDistinct_NonEmptyDistinctGuids() {
            var generator = new GuidGenerator();
            var original = generator.Next();

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var distinct = generator.NextDistinct(original);

                Assert.NotEqual(Guid.Empty, distinct);
                Assert.NotEqual(original, distinct);
            }
        }

    }

}
