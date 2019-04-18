namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasValues<out TSyntax> where TSyntax : ISqlSyntax {
        string ValuesRaw { get; }
        TSyntax Values(string clause);
    }

}