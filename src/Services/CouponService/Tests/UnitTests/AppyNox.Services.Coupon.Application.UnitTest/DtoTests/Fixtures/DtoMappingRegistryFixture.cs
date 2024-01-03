using AppyNox.Services.Coupon.Application.Dtos.DtoUtilities;

namespace AppyNox.Services.Coupon.Application.UnitTest.DtoTests.Fixtures
{
    public class DtoMappingRegistryFixture : IDisposable
    {
        #region [ Public Constructors ]

        public DtoMappingRegistryFixture()
        {
            Registry = new DtoMappingRegistry();
        }

        #endregion

        #region [ Properties ]

        public DtoMappingRegistry Registry { get; }

        #endregion

        #region [ Public Methods ]

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}