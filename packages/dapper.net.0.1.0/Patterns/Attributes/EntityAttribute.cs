using System;

namespace Dapper.Net.Patterns.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EntityAttribute : Attribute
    {
        public string EntityName { get; set; }

        public EntityAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
