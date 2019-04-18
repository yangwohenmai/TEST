using Dapper.Net.Patterns.Chainable.Syntax.Object;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasExpression {
        ISqlExpression ExpressionRaw { get; }
    }

}