using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Application.Dtos
{
    /// <summary>
    /// Represents the base class for update data transfer objects (DTOs).
    /// </summary>
    public class BaseUpdateDto : IUpdateDto
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the entity to be updated.
        /// </summary>
        public Guid Id { get; set; }

        #endregion
    }
}