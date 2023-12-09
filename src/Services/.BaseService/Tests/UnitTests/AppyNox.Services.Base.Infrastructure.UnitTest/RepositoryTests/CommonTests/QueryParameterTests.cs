using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Base.Domain.Helpers;
using AppyNox.Services.Base.Infrastructure.Repositories.Common;

namespace AppyNox.Services.Base.Infrastructure.UnitTest.RepositoryTests.CommonTests
{
    public class QueryParameterTests
    {
        #region [ Fields ]

        private const string _uncommonDetail = "UncommonDetail";

        #endregion

        #region [ Public Constructors ]

        [Fact]
        public void BaseQueryParameterInitializationShouldBeCorrect()
        {
            QueryParameters queryParameters = new();

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(CommonDtoLevelEnums.Simple.GetDisplayName(), queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        #endregion

        #region [ DataAccess Tests ]

        /// <summary>
        /// Access = ""
        /// DetailLevel = "Simple"
        /// </summary>
        [Fact]
        public void DataAccess_InitializationForSimpleShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = CommonDtoLevelEnums.Simple.GetDisplayName()
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(CommonDtoLevelEnums.Simple.GetDisplayName(), queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = ""
        /// DetailLevel = "simple"
        /// for case insensitive testing
        /// </summary>
        [Fact]
        public void DataAccess_InitializationForSimpleDetailLevelCaseInsensitiveShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "simple"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(CommonDtoLevelEnums.Simple.GetDisplayName(), queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "DataAccess"
        /// DetailLevel = "simple"
        /// </summary>
        [Fact]
        public void DataAccess_InitializationForSimpleDetailLevelCaseInsensitiveShouldBeCorrectWithAccess()
        {
            QueryParameters queryParameters = new()
            {
                Access = DtoLevelMappingTypes.DataAccess.GetDisplayName(),
                DetailLevel = "simple"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(CommonDtoLevelEnums.Simple.GetDisplayName(), queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = ""
        /// DetailLevel = "IdOnly"
        /// </summary>
        [Fact]
        public void DataAccess_InitializationForIdOnlyShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = CommonDtoLevelEnums.IdOnly.GetDisplayName()
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.IdOnly, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(CommonDtoLevelEnums.IdOnly.GetDisplayName(), queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = ""
        /// DetailLevel = "idonly"
        /// for case insensitive testing
        /// </summary>
        [Fact]
        public void DataAccess_InitializationForIdOnlyDetailLevelCaseInsensitiveShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = "idonly"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.IdOnly, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(CommonDtoLevelEnums.IdOnly.GetDisplayName(), queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "dataaccess"
        /// DetailLevel = "idonly"
        /// for case insensitive testing
        /// </summary>
        [Fact]
        public void DataAccess_InitializationForIdOnlyDetailLevelAndAccessCaseInsensitiveShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = "dataaccess",
                DetailLevel = "idonly"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.IdOnly, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(CommonDtoLevelEnums.IdOnly.GetDisplayName(), queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = ""
        /// DetailLevel = "UncommonDetail"
        /// </summary>
        [Fact]
        public void DataAccess_InitializationForNoneShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = string.Empty,
                DetailLevel = _uncommonDetail
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.None, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal(_uncommonDetail, queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        #endregion

        #region [ Update Tests ]

        /// <summary>
        /// Access = "Update"
        /// DetailLevel = ""
        /// </summary>
        [Fact]
        public void Update_InitializationForSimpleShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = DtoLevelMappingTypes.Update.GetDisplayName(),
                DetailLevel = string.Empty
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Update, queryParameters.AccessType);
            Assert.Equal("Simple", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "Update"
        /// DetailLevel = "Simple"
        /// </summary>
        [Fact]
        public void Update_InitializationForSimpleWithAccessAndDetailLevelShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = DtoLevelMappingTypes.Update.GetDisplayName(),
                DetailLevel = CommonDtoLevelEnums.Simple.GetDisplayName()
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Update, queryParameters.AccessType);
            Assert.Equal("Simple", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "update"
        /// DetailLevel = "simple"
        /// for case insensitive testing
        /// </summary>
        [Fact]
        public void Update_InitializationForSimpleDetailLevelAndAccessCaseInsensitiveShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = "update",
                DetailLevel = "simple"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Update, queryParameters.AccessType);
            Assert.Equal("Simple", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "update"
        /// DetailLevel = "Extended"
        /// </summary>
        [Fact]
        public void Update_InitializationForExtendedShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = "update",
                DetailLevel = "Extended"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.None, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Update, queryParameters.AccessType);
            Assert.Equal("Extended", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        #endregion

        #region [ Create Tests ]

        /// <summary>
        /// Access = "Create"
        /// DetailLevel = ""
        /// </summary>
        [Fact]
        public void Create_InitializationForSimpleShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = DtoLevelMappingTypes.Create.GetDisplayName(),
                DetailLevel = string.Empty
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Create, queryParameters.AccessType);
            Assert.Equal("Simple", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "Create"
        /// DetailLevel = "Simple"
        /// </summary>
        [Fact]
        public void Create_InitializationForSimpleWithAccessAndDetailLevelShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = DtoLevelMappingTypes.Create.GetDisplayName(),
                DetailLevel = CommonDtoLevelEnums.Simple.GetDisplayName()
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Create, queryParameters.AccessType);
            Assert.Equal("Simple", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "create"
        /// DetailLevel = "simple"
        /// for case insensitive testing
        /// </summary>
        [Fact]
        public void Create_InitializationForSimpleDetailLevelAndAccessCaseInsensitiveShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = "create",
                DetailLevel = "simple"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.Simple, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Create, queryParameters.AccessType);
            Assert.Equal("Simple", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        /// <summary>
        /// Access = "create"
        /// DetailLevel = "Extended"
        /// </summary>
        [Fact]
        public void Create_InitializationForExtendedAccessCaseInsensitiveShouldBeCorrect()
        {
            QueryParameters queryParameters = new()
            {
                Access = "create",
                DetailLevel = "Extended"
            };

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.None, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.Create, queryParameters.AccessType);
            Assert.Equal("Extended", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        #endregion

        #region [ Static Method Tests ]

        [Fact]
        public void CreateForIdOnly_SholdReturnIdOnlyQueryParameter()
        {
            QueryParameters queryParameters = (QueryParameters)QueryParameters.CreateForIdOnly();

            Assert.Equal(1, queryParameters.PageNumber);
            Assert.Equal(10, queryParameters.PageSize);
            Assert.Equal(CommonDtoLevelEnums.IdOnly, queryParameters.CommonDtoLevel);
            Assert.Equal(DtoLevelMappingTypes.DataAccess, queryParameters.AccessType);
            Assert.Equal("IdOnly", queryParameters.DetailLevel);
            Assert.Equal(string.Empty, queryParameters.Access);
        }

        #endregion
    }
}