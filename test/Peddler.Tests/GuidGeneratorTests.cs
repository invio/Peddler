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

    }

}
