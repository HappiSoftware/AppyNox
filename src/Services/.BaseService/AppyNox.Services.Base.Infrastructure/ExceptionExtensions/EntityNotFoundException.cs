namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions
{
    public class EntityNotFoundException<TEntity> : Exception
    {
        #region [ Public Constructors ]

        public EntityNotFoundException(Guid entityId)
            : base($"Entity of type {typeof(TEntity).Name} with ID {entityId} was not found.")
        {
            EntityId = entityId;
        }

        #endregion

        #region [ Properties ]

        public Guid EntityId { get; }

        #endregion
    }
}