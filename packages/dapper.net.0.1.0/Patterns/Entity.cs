using Tix.Services.Sql.Patterns.Metadata;

namespace Tix.Services.Sql.Patterns
{
    /// <summary>
    /// Allows representing a model as a SQL entity.  Do not inherit directly on models (for now).
    /// </summary>
    /// <typeparam name="TModel">Type of the primary key</typeparam>
    public interface IEntity<TModel>
    {
        TModel Model { get; }
        EntityMetadata<TModel> Metadata { get; }
    }

    /// <summary>
    /// A database entity with backing model and mappings.
    /// </summary>
    /// <typeparam name="TModel">Type of the model backing the entity.</typeparam>
    public class Entity<TModel> : IEntity<TModel>
    {
        public TModel Model { get; }
        public EntityMetadata<TModel> Metadata { get; }

        public Entity(TModel model, EntityMetadata<TModel> metadata = null)
        {
            Model = model;
            Metadata = metadata ?? new EntityMetadata<TModel>();
        }
    }
}
