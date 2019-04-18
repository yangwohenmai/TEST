using Dapper.Net.Patterns.Chainable.Syntax.Implements;
using Dapper.Net.Patterns.Syntax;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public interface ISqlJoin : ISqlClause,
                                IHasTable,
                                IHasOn<ISqlJoin>,
                                IHasJoin<ISqlJoin> {
        SqlJoinDirection DirectionRaw { get; }
    }

}