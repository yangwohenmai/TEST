namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasTop<out TSyntax> where TSyntax : ISqlSyntax {
        int? TopRaw { get; }
        TSyntax Top(int? top);
    }

}