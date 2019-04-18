namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasOrderBy<out TSyntax> where TSyntax : ISqlSyntax {
        string OrderByRaw { get; }
        TSyntax OrderBy(string clause);
    }

}