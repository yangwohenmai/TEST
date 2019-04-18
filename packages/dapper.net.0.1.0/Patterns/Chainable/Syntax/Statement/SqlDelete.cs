using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using S = Dapper.Net.Patterns.Syntax.SqlTokens;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public class SqlDelete : SqlStatement<ISqlDelete>, ISqlDelete {
        public override string ToSql() => $"{S.Delete} {G.Top(TopRaw)} {FromRaw.ToSql()} {G.Clause(WhereRaw)}";

        public int? TopRaw { get; private set; }
        public ISqlFrom FromRaw { get; private set; }
        public ISqlWhere WhereRaw { get; private set; }

        public ISqlDelete Top(int? top) => Chain(() => TopRaw = top);

        public ISqlDelete Where(ISqlExpression clause, object param) => Where(new SqlCondition(clause, param));
        public ISqlDelete Where(ISqlCondition clause) => Where(new SqlWhere(clause));
        public ISqlDelete Where(ISqlWhere clause) => Chain(() => WhereRaw = clause);
        public ISqlDelete WhereNot(ISqlCondition clause) => Where(SqlCondition.Not(clause));

        public ISqlDelete From(string clause) => From(new SqlFrom(clause));
        public ISqlDelete From(ISqlFrom clause) => Chain(() => FromRaw = clause);
    }

}