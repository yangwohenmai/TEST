using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Chainable.Syntax.Object;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasOn<out TSyntax> where TSyntax : ISqlSyntax {
        ISqlOn OnRaw { get; }
        TSyntax On(ISqlExpression clause, object param = null);
        TSyntax On(ISqlCondition clause);
        TSyntax On(ISqlOn clause);
    }

}