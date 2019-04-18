namespace Dapper.Net.Patterns.Chainable.Syntax.Object {

    public class SqlTable : ISqlTable {
        public string ToSql() => $"[{TableRaw}] [{AliasRaw}]";

        public SqlTable(string tableRaw, string aliasRaw = null) {
            TableRaw = tableRaw;
            AliasRaw = aliasRaw ?? tableRaw;
        }

        public string TableRaw { get; }
        public string AliasRaw { get; }
    }

}