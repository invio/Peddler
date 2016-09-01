using System;

namespace Peddler {

    public class Int64GeneratorTests : IntegralGeneratorTests<Int64> {

        protected override IntegralGenerator<Int64> CreateGenerator() {
            return new Int64Generator();
        }

        protected override IntegralGenerator<Int64> CreateGenerator(Int64 low) {
            return new Int64Generator(low);
        }

        protected override IntegralGenerator<Int64> CreateGenerator(Int64 low, Int64 high) {
            return new Int64Generator(low, high);
        }

    }

}
