namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasNoLock<out TSyntax> where TSyntax : ISqlSyntax {
        bool NoLockRaw { get; }
        TSyntax NoLock();
    }

}