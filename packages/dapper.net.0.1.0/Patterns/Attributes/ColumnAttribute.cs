using System;

namespace Dapper.Net.Patterns.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }
		public bool IsPrimaryKey { get; set; }

        public ColumnAttribute(string columnName = null, bool isPrimaryKey = false)
        {
            ColumnName = columnName;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}
