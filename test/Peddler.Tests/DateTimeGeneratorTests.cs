using System;

namespace Peddler {

    public class DateTimeGeneratorTests : IntegralGeneratorTests<DateTime> {

        protected override IIntegralGenerator<DateTime> CreateGenerator() {
            return new DateTimeGenerator();
        }

        protected override IIntegralGenerator<DateTime> CreateGenerator(DateTime low) {
            return new DateTimeGenerator(low);
        }

        protected override IIntegralGenerator<DateTime> CreateGenerator(
            DateTime low,
            DateTime high) {

            return new DateTimeGenerator(low, high);
        }

    }

}
