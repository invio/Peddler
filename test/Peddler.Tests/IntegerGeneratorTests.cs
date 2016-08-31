using System;
using Xunit;

namespace Peddler {

    public class IntegerGeneratorTests : IntegralGeneratorTests<int> {

        protected override IntegralGenerator<int> CreateGenerator() {
            return new IntegerGenerator();
        }

        protected override IntegralGenerator<int> CreateGenerator(int low) {
            return new IntegerGenerator(low);
        }

        protected override IntegralGenerator<int> CreateGenerator(int low, int high) {
            return new IntegerGenerator(low, high);
        }

    }


}
