using System;

namespace Peddler {

    public class UInt64GeneratorTests : IntegralGeneratorTests<UInt64> {

        protected override IntegralGenerator<UInt64> CreateGenerator() {
            return new UInt64Generator();
        }

        protected override IntegralGenerator<UInt64> CreateGenerator(UInt64 low) {
            return new UInt64Generator(low);
        }

        protected override IntegralGenerator<UInt64> CreateGenerator(UInt64 low, UInt64 high) {
            return new UInt64Generator(low, high);
        }

    }

}
