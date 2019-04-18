namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public abstract class SqlClause<T> : ChainableSql<T>, ISqlClause where T : ISqlClause {
    }

}