using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using S = Dapper.Net.Patterns.Syntax.SqlTokens;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public class SqlUpdate : SqlStatement<ISqlUpdate>, ISqlUpdate {
        public override string ToSql() => $"{S.Update} {G.Top(TopRaw)} {TableRaw.ToSql()} {G.Clause(SetRaw)} {G.Clause(FromRaw)} {G.Clause(WhereRaw)}".TrimSqueeze();
        public int? TopRaw { get; set; }
        public ISqlTable TableRaw { get; }
        public ISqlSet SetRaw { get; private set; }
        public ISqlFrom FromRaw { get; private set; }
        public ISqlWhere WhereRaw { get; private set; }

        public SqlUpdate(ISqlTable table) {
            TableRaw = table;
        }

        public ISqlUpdate Top(int? top) => Chain(() => TopRaw = top);
        public ISqlUpdate Set(ISqlSet clause) => Chain(() => SetRaw = clause);
        public ISqlUpdate From(ISqlFrom clause) => Chain(() => FromRaw = clause);
        public ISqlUpdate Where(ISqlExpression clause, object template = null) => Where(new SqlCondition(clause, template));
        public ISqlUpdate Where(ISqlCondition clause) => Where(new SqlWhere(clause));
        public ISqlUpdate Where(ISqlWhere clause) => Chain(() => WhereRaw = clause);
        public ISqlUpdate WhereNot(ISqlCondition clause) => Where(SqlCondition.Not(clause));
    }

}