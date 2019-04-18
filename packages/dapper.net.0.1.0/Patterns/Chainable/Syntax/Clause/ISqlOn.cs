using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public interface ISqlOn : ISqlClause,
                              IHasCondition<ISqlOn> {}

}