namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasDistinct<out TSyntax> where TSyntax : ISqlSyntax {
        bool DistinctRaw { get; }
        TSyntax Distinct();
    }

}