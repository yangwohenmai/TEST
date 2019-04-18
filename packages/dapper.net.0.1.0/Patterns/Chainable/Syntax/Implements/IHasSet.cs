using Dapper.Net.Patterns.Chainable.Syntax.Clause;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasSet<out TSyntax> where TSyntax : ISqlSyntax {
        ISqlSet SetRaw { get; }
        TSyntax Set(ISqlSet clause);
    }

}