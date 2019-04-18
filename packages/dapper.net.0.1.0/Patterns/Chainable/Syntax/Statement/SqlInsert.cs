using Dapper.Net.Patterns.Chainable.Syntax.Object;
using S = Dapper.Net.Patterns.Syntax.SqlTokens;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public class SqlInsert : SqlStatement<ISqlInsert>, ISqlInsert {
        public override string ToSql() => $"{S.Insert} {G.Top(TopRaw)} {S.Into} {TableRaw.ToSql()} ({ColumnsRaw}) {G.Values(ValuesRaw)}";

        public ISqlTable TableRaw { get; }
        public int? TopRaw { get; private set; }
        public string ColumnsRaw { get; private set; }
        public string ValuesRaw { get; private set; }

        public SqlInsert(ISqlTable table) {
            TableRaw = table;
        }

        public ISqlInsert Top(int? top) => Chain(() => TopRaw = top);
        public ISqlInsert Columns(string clause) => Chain(() => ColumnsRaw = clause);
        public ISqlInsert Values(string clause) => Chain(() => ValuesRaw = clause);
    }

}