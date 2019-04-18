using Dapper.Net.Patterns.Chainable.Syntax.Clause;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasWhere<out TSyntax> where TSyntax : ISqlSyntax {
        ISqlWhere WhereRaw { get; }
        TSyntax Where(ISqlCondition clause);
        TSyntax Where(ISqlWhere clause);
        TSyntax WhereNot(ISqlCondition clause);
    }

}