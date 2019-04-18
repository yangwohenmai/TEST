using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public interface ISqlDelete : ISqlStatement,
                                  IHasTop<ISqlDelete>,
                                  IHasFrom<ISqlDelete>,
                                  IHasWhere<ISqlDelete> {}

}