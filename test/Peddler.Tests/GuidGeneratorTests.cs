using System;
using System.Collections.Generic;
using Xunit;

namespace Peddler {

    public class GuidGeneratorTests {

        private const int numberOfAttempts = 100;

        [Fact]
        public void Next_NonEmptyGuids() {
            var generator = new GuidGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.NotEqual(Guid.Empty, value);
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        [Fact]
        public void Next_Uniqueness() {
            var generator = new GuidGenerator();
            var values = new HashSet<Guid>();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.True(
                    values.Add(value),
                    $"SequentialGuidGenerator generated the value '{value}' several times."
                );
            }
        }

        [Fact]
        public void NextDistinct_NonEmptyDistinctGuids() {
            var generator = new GuidGenerator();
            var original = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var distinct = generator.NextDistinct(original);

                Assert.NotEqual(Guid.Empty, distinct);
                Assert.NotEqual(original, distinct);
                Assert.False(generator.EqualityComparer.Equals(original, distinct));
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
