using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public interface ISqlCondition : ISqlClause,
                                     IBuildsParam,
                                     IHasParam,
                                     IHasInversion,
                                     IHasExpression,
                                     IHasAndCondition<ISqlCondition>,
                                     IHasOrCondition<ISqlCondition> {}

}