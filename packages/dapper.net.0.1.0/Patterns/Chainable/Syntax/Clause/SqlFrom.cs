using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using S = Dapper.Net.Patterns.Syntax.SqlTokens;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;
using J = Dapper.Net.Patterns.Syntax.SqlJoinDirection;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public class SqlFrom : SqlClause<ISqlFrom>, ISqlFrom {
        public override sealed string ToSql() => $"{S.From} {TableRaw.ToSql()} {G.NoLock(NoLockRaw)} {G.Clause(JoinRaw)}".TrimSqueeze();
        public ISqlTable TableRaw { get; }
        public bool NoLockRaw { get; private set; }
        public ISqlJoin JoinRaw { get; private set; }

        public ISqlFrom InnerJoin(ISqlTable clause, ISqlOn onClause) => Join(J.Inner, clause, onClause);
        public ISqlFrom LeftJoin(ISqlTable clause, ISqlOn onClause) => Join(J.Left, clause, onClause);
        public ISqlFrom RightJoin(ISqlTable clause, ISqlOn onClause) => Join(J.Right, clause, onClause);
        public ISqlFrom OuterJoin(ISqlTable clause, ISqlOn onClause) => Join(J.Outer, clause, onClause);
        public ISqlFrom Join(J direction, ISqlTable clause, ISqlOn onClause) => Join(new SqlJoin(direction, clause, onClause));
        public ISqlFrom Join(ISqlJoin clause) => Chain(() => JoinRaw = clause);
        public ISqlFrom NoLock() => Chain(() => NoLockRaw = true);

        public SqlFrom(string tableName, string aliasName = null) {
            TableRaw = new SqlTable(tableName, aliasName);
        }

        public SqlFrom(ISqlTable table) {
            TableRaw = table;
        }

    }

}