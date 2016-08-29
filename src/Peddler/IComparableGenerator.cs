using System;

namespace Peddler {

    // Generates a different 'T' from the one provided.
    // It throws 'InvalidOperationException()' if it can't do it.
    public interface IComparableGenerator<T> : IGenerator<T> {

        T NextGreaterThan(T other);
        T NextGreaterThanOrEqualTo(T other);
        T NextLessThan(T other);
        T NextLessThanOrEqualTo(T other);

    }

}
