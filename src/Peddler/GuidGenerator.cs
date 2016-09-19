using System;

namespace Peddler {

    /// <summary>
    ///   A basic implementation of <see cref="IDistinctGenerator{Guid}" />
    ///   for creating instances of <see cref="Guid" />.
    /// </summary>
    public class GuidGenerator : IDistinctGenerator<Guid> {

        /// <summary>
        ///   Generates a new, non-empty <see cref="Guid" /> instance.
        /// </summary>
        /// <remarks>
        ///   This utilizes the .NET Framework's built-in implementation of
        ///   <see cref="Guid" /> generation.
        /// </remarks>
        public virtual Guid Next() {
            return Guid.NewGuid();
        }

        /// <summary>
        ///   Generates a new, non-empty, distinct <see cref="Guid" /> instance.
        /// </summary>
        /// <params name="other">
        ///   A <see cref="Guid" /> the caller wants to be distinct from the
        ///   <see cref="Guid" /> that is returned.
        /// </params>
        /// <returns>
        ///   A new <see cref="Guid" /> that is distinct from the one provided via
        ///   the <paramref name="other" /> parameter.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this <see cref="IDistinctGenerator{T}"/> is unable to
        ///   generate a value that is distinct from <paramref name="other" />.
        /// </exception>
        public Guid NextDistinct(Guid other) {
            var next = this.Next();

            if (next == other) {
                throw new UnableToGenerateValueException(
                    $"A {typeof(Guid).Name} was generated with the same value as the " +
                    $"{typeof(Guid).Name} provided ({other:B}). This most likely means there " +
                    $"is a flaw with this system's {typeof(Guid).Name} generation algorithm."
                );
            }

            return next;
        }

    }

}
