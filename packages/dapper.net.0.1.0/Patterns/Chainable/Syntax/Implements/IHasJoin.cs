using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using J = Dapper.Net.Patterns.Syntax.SqlJoinDirection;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasJoin<out TSyntax> where TSyntax : ISqlSyntax {
        ISqlJoin JoinRaw { get; }
        TSyntax InnerJoin(ISqlTable obj, ISqlOn clause);
        TSyntax LeftJoin(ISqlTable obj, ISqlOn clause);
        TSyntax RightJoin(ISqlTable obj, ISqlOn clause);
        TSyntax OuterJoin(ISqlTable obj, ISqlOn clause);
        TSyntax Join(J direction, ISqlTable obj, ISqlOn clause);
        TSyntax Join(ISqlJoin clause);
    }

}