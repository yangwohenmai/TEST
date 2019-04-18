using System.Collections.Generic;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using Dapper.Net.Validation;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    public abstract class SqlStatement<T> : ChainableSql<T>, ISqlStatement where T : ISqlStatement {

        private readonly ISqlParser _parser;

        protected SqlStatement(ISqlParser parser = null) {
            _parser = parser ?? new SqlParser();
        }

        public object GetParam() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetErrors(string sql = null) => _parser.ParseErrors(sql ?? ToSql());

        public static ISqlSelect Select(string columns) => new SqlSelect(columns);
        public static ISqlUpdate Update(string tableName, string aliasName = null) => Update(new SqlTable(tableName, aliasName));
        public static ISqlUpdate Update(ISqlTable table) => new SqlUpdate(table);
        public static ISqlInsert Insert(string tableName, string aliasName = null) => Insert(new SqlTable(tableName, aliasName));
        public static ISqlInsert Insert(ISqlTable table) => new SqlInsert(table);
        public static ISqlDelete Delete() => new SqlDelete();
    }

}