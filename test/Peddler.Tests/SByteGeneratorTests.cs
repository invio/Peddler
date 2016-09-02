using System;

namespace Peddler {

    public class SByteGeneratorTests : IntegralGeneratorTests<SByte> {

        protected override IIntegralGenerator<SByte> CreateGenerator() {
            return new SByteGenerator();
        }

        protected override IIntegralGenerator<SByte> CreateGenerator(SByte low) {
            return new SByteGenerator(low);
        }

        protected override IIntegralGenerator<SByte> CreateGenerator(SByte low, SByte high) {
            return new SByteGenerator(low, high);
        }

    }

}
