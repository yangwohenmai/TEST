using Dapper.Net.Patterns.Chainable.Syntax.Clause;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasCondition<out TSyntax> : IHasAndCondition<TSyntax>,
                                                  IHasOrCondition<TSyntax> where TSyntax : ISqlSyntax {
        ISqlCondition ConditionRaw { get; }
    }

}