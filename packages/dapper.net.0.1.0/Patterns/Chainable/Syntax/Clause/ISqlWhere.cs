using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public interface ISqlWhere : ISqlClause,
                                 IBuildsParam,
                                 IHasCondition<ISqlWhere> {}

}