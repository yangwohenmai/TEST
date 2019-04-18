using System;

namespace Dapper.Net.Patterns.Chainable {

    public interface IChainable<out T>
    {
        T Chain(Action action);
        T Get();
    }

}