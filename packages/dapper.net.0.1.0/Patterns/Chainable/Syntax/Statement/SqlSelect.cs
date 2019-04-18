using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Syntax;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public class SqlSelect : SqlStatement<ISqlSelect>, ISqlSelect {
        public override string ToSql() => $"{SqlTokens.Select} {Top(TopRaw)} {G.Distinct(DistinctRaw)} {ColumnsRaw} {G.Clause(FromRaw)} {G.Clause(WhereRaw)}".TrimSqueeze();

        public SqlSelect(string columns) {
            ColumnsRaw = columns;
        }

        public string ColumnsRaw { get; }
        public int? TopRaw { get; private set; }
        public bool DistinctRaw { get; private set; }
        public ISqlFrom FromRaw { get; private set; }
        public ISqlWhere WhereRaw { get; private set; }
        public string GroupByRaw { get; private set; }
        public string OrderByRaw { get; private set; }

        public ISqlSelect Top(int? top) => Chain(() => TopRaw = top);
        public ISqlSelect Distinct() => Chain(() => DistinctRaw = true);
        public ISqlSelect From(string clause) => From(new SqlFrom(clause));
        public ISqlSelect From(ISqlFrom clause) => Chain(() => FromRaw = clause);
        public ISqlSelect Where(ISqlCondition clause) => Where(new SqlWhere(clause));
        public ISqlSelect Where(ISqlWhere clause) => Chain(() => WhereRaw = clause);
        public ISqlSelect WhereNot(ISqlCondition clause) => Where(SqlCondition.Not(clause));

        public ISqlSelect GroupBy(string clause) => Chain(() => GroupByRaw = clause);
        public ISqlSelect OrderBy(string clause) => Chain(() => OrderByRaw = clause);
    }

}