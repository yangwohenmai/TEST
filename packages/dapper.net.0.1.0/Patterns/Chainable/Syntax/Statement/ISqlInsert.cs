using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public interface ISqlInsert : ISqlStatement,
                                  IHasTable,
                                  IHasTop<ISqlInsert>,
                                  IHasColumns<ISqlInsert>,
                                  IHasValues<ISqlInsert> {}

}