using System;

namespace Peddler {

    public class GuidGenerator : IGenerator<Guid> {

        public Guid Next() {
            return Guid.NewGuid();
        }

    }

}
