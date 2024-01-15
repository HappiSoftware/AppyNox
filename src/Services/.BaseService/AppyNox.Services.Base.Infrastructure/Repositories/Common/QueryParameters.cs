using AppyNox.Services.Base.Core.Enums;

namespace AppyNox.Services.Base.Infrastructure.Repositories.Common
{
    /// <summary>
    /// Represents specific query parameters for data retrieval.
    /// </summary>
    public class QueryParameters : QueryParametersBase
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameters"/> class with default settings.
        /// </summary>
        public QueryParameters() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameters"/> class with a specified common DTO level.
        /// </summary>
        /// <param name="commonDtoLevel">The common DTO level.</param>
        protected QueryParameters(CommonDtoLevelEnums commonDtoLevel) : base(commonDtoLevel)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="QueryParametersBase"/> for queries that require only the ID.
        /// </summary>
        /// <returns>A new <see cref="QueryParametersBase"/> instance with settings for ID-only queries.</returns>
        public new static QueryParametersBase CreateForIdOnly()
        {
            return new QueryParameters(CommonDtoLevelEnums.IdOnly);
        }

        #endregion
    }
}