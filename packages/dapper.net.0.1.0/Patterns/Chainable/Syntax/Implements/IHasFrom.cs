using Dapper.Net.Patterns.Chainable.Syntax.Clause;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasFrom<out TSyntax> where TSyntax : ISqlSyntax {
        ISqlFrom FromRaw { get; }
        TSyntax From(ISqlFrom clause);
    }

}