using System;

namespace Peddler {

    // Generates a different 'T' from the one provided.
    // It throws 'InvalidOperationException()' if it can't do it.
    public interface IDistinctGenerator<T> : IGenerator<T> {

        T NextDistinct(T other);

    }


}
