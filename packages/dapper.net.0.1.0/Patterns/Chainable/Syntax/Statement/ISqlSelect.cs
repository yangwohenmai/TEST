using Dapper.Net.Patterns.Chainable.Syntax.Implements;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public interface ISqlSelect : ISqlStatement,
                                  IHasTop<ISqlSelect>,
                                  IHasDistinct<ISqlSelect>,
                                  IHasFrom<ISqlSelect>,
                                  IHasWhere<ISqlSelect>,
                                  IHasGroupBy<ISqlSelect>,
                                  IHasOrderBy<ISqlSelect> {}

}