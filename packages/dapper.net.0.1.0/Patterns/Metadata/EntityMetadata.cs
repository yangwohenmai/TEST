using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.Net.Patterns.Attributes;
using Dapper.Net.Patterns.Exceptions;

namespace Dapper.Net.Patterns.Metadata {

    public class EntityMetadata {
        public string ModelName { get; private set; }
        public string EntityName { get; private set; }

        public EntityMetadata(string modelName, string entityName) {
            ModelName = modelName;
            EntityName = entityName;
        }
    }

    public class EntityMetadata<TModel> {
        public Type ModelType { get; set; }
        public EntityMetadata Entity { get; set; }
        public ColumnMetadata PrimaryKey { get; set; }
        public IEnumerable<ColumnMetadata> Columns { get; set; }

        public EntityMetadata() {
            ModelType = typeof (TModel);
            Reflect();
        }

        private void Reflect() {
            Entity = GetEntity();
            Columns = GetColumns();
            PrimaryKey = GetPrimaryKey();
        }

        private EntityMetadata GetEntity() {
            var modelName = ModelType.Name;
            var entityAttribute = ModelType.GetCustomAttribute<EntityAttribute>();
            var entityName = entityAttribute == null ? modelName : entityAttribute.EntityName;
            return new EntityMetadata(modelName, entityName);
        }

        private IEnumerable<ColumnMetadata> GetColumns() {
            foreach (var column in ModelType.GetProperties(BindingFlags.Public)) {
                var modelType = column.PropertyType;
                var modelName = column.Name;
                var columnAttribute = column.GetCustomAttribute<ColumnAttribute>();
                yield return columnAttribute == null
                    ? new ColumnMetadata(modelType, modelName)
                    : new ColumnMetadata(modelType, modelName, columnAttribute.ColumnName, columnAttribute.IsPrimaryKey);
            }
        }

        private ColumnMetadata GetPrimaryKey() {
            var primaryKey = Columns.SingleOrDefault(c => c.IsPrimaryKey);
            if (primaryKey == null)
                throw new SqlPatternException(SqlPatternException.SqlPatternExceptionType.NoPrimaryKeyDefined);
            return primaryKey;
        }
    }

}