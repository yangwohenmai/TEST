using Dapper.Net.Extensions;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public class SqlSet : SqlClause<ISqlSet>, ISqlSet
    {
        public string ClauseRaw { get; }

        public SqlSet(string clause) {
            ClauseRaw = clause;
        }

        public override string ToSql() => ClauseRaw.TrimSqueeze();
    }

}