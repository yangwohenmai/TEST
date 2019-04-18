using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public interface ISqlFrom : ISqlClause,
                                IHasTable,
                                IHasNoLock<ISqlFrom>,
                                IHasJoin<ISqlFrom> {
    }

}