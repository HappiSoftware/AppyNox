using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.Infrastructure.ExceptionExtensions
{
    public class EntityNotFoundException<TEntity> : Exception
    {
        public Guid EntityId { get; }

        public EntityNotFoundException(Guid entityId)
            : base($"Entity of type {typeof(TEntity).Name} with ID {entityId} was not found.")
        {
            EntityId = entityId;
        }
    }
}
