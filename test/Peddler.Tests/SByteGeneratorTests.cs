using System;

namespace Peddler {

    public class SByteGeneratorTests : IntegralGeneratorTests<SByte> {

        protected override IntegralGenerator<SByte> CreateGenerator() {
            return new SByteGenerator();
        }

        protected override IntegralGenerator<SByte> CreateGenerator(SByte low) {
            return new SByteGenerator(low);
        }

        protected override IntegralGenerator<SByte> CreateGenerator(SByte low, SByte high) {
            return new SByteGenerator(low, high);
        }

    }

}
