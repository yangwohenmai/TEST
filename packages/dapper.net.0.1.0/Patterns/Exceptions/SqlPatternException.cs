using System;
using System.ComponentModel;
using Dapper.Net.Extensions;

namespace Dapper.Net.Patterns.Exceptions {

    public class SqlPatternException : Exception {

        public enum SqlPatternExceptionType {
            [Description("No primary key was defined for the entity.")]
            NoPrimaryKeyDefined,
            [Description("Could not parse FROM clause.")]
            CouldNotParseFromClause,
        }

        public SqlPatternException(SqlPatternExceptionType type, Exception innerException = null)
            : base(type.GetDescription(), innerException) { }

    }

}