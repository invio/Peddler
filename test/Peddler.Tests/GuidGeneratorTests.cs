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

        [Fact]
        public void NextDistinct_DuplicateGuid() {
            var generator = new ConstantGuidGenerator();
            var original = generator.Next();

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(original)
            );
        }

        private class ConstantGuidGenerator : GuidGenerator {

            public override Guid Next() {
                return new Guid("1ab32a76-45dd-4e50-93d8-59642dcd8823");
            }

        }

    }

}
