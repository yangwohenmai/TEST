using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Chainable.Syntax.Object;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasAndCondition<out TSyntax> where TSyntax : ISqlSyntax {
        ISqlCondition AndRaw { get; }

        TSyntax And(ISqlExpression clause, object param = null);
        TSyntax And(ISqlCondition clause);

        TSyntax AndNot(ISqlExpression clause, object param = null);
        TSyntax AndNot(ISqlCondition clause);
    }

}