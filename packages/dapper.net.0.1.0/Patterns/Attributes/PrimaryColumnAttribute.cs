using System;

namespace Dapper.Net.Patterns.Attributes {

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryColumnAttribute : ColumnAttribute {
        public PrimaryColumnAttribute(string columnName = null) {
            ColumnName = columnName;
            IsPrimaryKey = true;
        }
    }

}