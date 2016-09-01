using System;

namespace Peddler {

    /// <summary>
    ///   A basic <see cref="T:IGenerator`1" /> implementation for <see cref="T:System.Guid" />.
    /// </summary>
    public class GuidGenerator : IGenerator<Guid> {

        /// <summary>
        ///   Generates a new, non-empty <see cref="T:System.Guid" /> instance.
        /// </summary>
        /// <remarks>
        ///   This utilizes the built-in .NET Framework's implementation of GUID generation.
        /// </remarks>
        public Guid Next() {
            return Guid.NewGuid();
        }

    }

}
