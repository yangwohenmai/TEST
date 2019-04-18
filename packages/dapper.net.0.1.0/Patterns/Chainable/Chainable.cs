using System;

namespace Dapper.Net.Patterns.Chainable {

    public class Chainable<T> : IChainable<T> {
        private readonly T _derived;

        public Chainable(T derived) {
            _derived = derived;
        }

        public T Chain(Action action) {
            action();
            return Get();
        }

        public T Get() => _derived;
    }

}