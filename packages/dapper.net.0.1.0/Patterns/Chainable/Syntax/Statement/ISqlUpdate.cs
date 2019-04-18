using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public interface ISqlUpdate : ISqlStatement,
                                  IHasTable,
                                  IHasTop<ISqlUpdate>,
                                  IHasSet<ISqlUpdate>,
                                  IHasFrom<ISqlUpdate>,
                                  IHasWhere<ISqlUpdate> {}

}