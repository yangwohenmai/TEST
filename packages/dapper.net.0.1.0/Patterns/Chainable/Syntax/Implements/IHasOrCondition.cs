using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Chainable.Syntax.Object;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasOrCondition<out TSyntax> where TSyntax : ISqlSyntax {
        ISqlCondition OrRaw { get; }

        TSyntax Or(ISqlExpression clause, object param = null);
        TSyntax Or(ISqlCondition clause);

        TSyntax OrNot(ISqlExpression clause, object param = null);
        TSyntax OrNot(ISqlCondition clause);
    }

}