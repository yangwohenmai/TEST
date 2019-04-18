using System.Collections.Generic;

namespace Dapper.Net.Patterns.Chainable.Syntax.Statement {

    /// <summary>
    /// Top level interface for representing chainable sql statements
    /// <remarks>
    /// 1:1 with SQL Data Manipulation Language (DML) statements
    /// </remarks>
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/library/ms177563.aspx"/>
    /// <see cref="https://msdn.microsoft.com/en-us/library/ff848766.aspx"/>
    public interface ISqlStatement : ISqlSyntax {
        object GetParam();
        IEnumerable<string> GetErrors(string sql = null);
    }

}