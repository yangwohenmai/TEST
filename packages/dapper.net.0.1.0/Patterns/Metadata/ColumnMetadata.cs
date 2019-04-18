using System;

namespace Dapper.Net.Patterns.Metadata {

    /// <summary>
    /// Tracks metadata for a SQL column.  Maps the field name on the model to the database name.
    /// </summary>
    public class ColumnMetadata {
        public Type AttributeType { get; private set; }
        public bool IsPrimaryKey { get; }
        public string ModelName { get; private set; }
        public string ColumnName { get; private set; }

        public ColumnMetadata(Type attributeType, string modelName, string columnName = null, bool isPrimaryKey = false) {
            AttributeType = attributeType;
            ModelName = modelName;
            ColumnName = columnName ?? modelName;
            IsPrimaryKey = isPrimaryKey;
        }
    }

}
