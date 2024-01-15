# Auditable Data Usage

Nox is handling simple Auditable Data operations automatically. However for enabling this feature some steps has to be done first. Please follow these:

1. **IAuditableData interface should be added to entity class.**

<details>
<summary>Click to expand the example</summary>

```cs
namespace AppyNox.Services.Coupon.Domain.Entities
{
    public class CouponEntity : IEntityWithGuid, IAuditableData
    {
        // omitted

        #region [ IAuditableData ]

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdateDate { get; set; }

        #endregion

        // omitted
    }
}
```

</details>

<br>

2. **IAuditDto interface should be added to dto class.**

<details>
<summary>Click to expand the example</summary>

```cs
namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base
{
    [CouponDetailLevel(CouponDataAccessDetailLevel.Simple)]
    public class CouponSimpleDto : CouponSimpleCreateDto, IAuditDto
    {
        #region [ IAuditDto ]

        public AuditInfo AuditInfo { get; set; } = null!;

        #endregion
    }
}
```

</details>

<br>

3. **In Dto Mapping `MapAuditInfo` should be used**

<details>
<summary>Click to expend the example</summary>

```cs
namespace AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Mappings
{
    public class CouponMapping : Profile
    {
        #region [ Public Constructors ]

        public CouponMapping()
        {
            // omitted
            CreateMap<CouponEntity, CouponSimpleDto>().MapAuditInfo().ReverseMap();
            // omitted
        }

        #endregion
    }
}
```

</details>

4. **During saving UnitOfWorkBase should be used.** <br>
   A UnitOfWork which inherits UnitOfWorkBase should be used.

5. **During saving UserId should be passed to SaveAsynch** <br>
   UnitOfWorkBase `SetCurrentUser(string userId)` should be called before calling `SaveChangesAsync()` or UserId should be passed to `SaveChangesAsync(string userId)`
