using System;

namespace Peddler {

    public interface IGenerator<out T> {

        T Next();

    }

}
