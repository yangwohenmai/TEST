using System.Collections.Generic;

namespace Dapper.Net.Validation {

    public interface ISqlParser {
        IEnumerable<string> ParseErrors(string sql);
    }

}