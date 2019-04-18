using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Dapper.Net.Validation {

    public class SqlParser : ISqlParser {
        public IEnumerable<string> ParseErrors(string sql) {
            var parser = new TSql120Parser(false);
            using (var reader = new StringReader(sql)) {
                IList<ParseError> errors;
                parser.Parse(reader, out errors);
                foreach (var error in errors) {
                    yield return error.Message;
                }
            }
        }
    }

}