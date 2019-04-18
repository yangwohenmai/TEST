using System;

namespace Dapper.Net.Patterns.Chainable {

    public abstract class ChainableSql<T> : ISqlSyntax where T : ISqlSyntax
    {
        private readonly Chainable<T> _chainable;

        protected ChainableSql() { 
            _chainable = new Chainable<T>((T) (ISqlSyntax)this);
        }

        public T Chain(Action action) => _chainable.Chain(action);
        public T Get() => _chainable.Get();
        public abstract string ToSql();
    }

}