# Auditable Data Usage

Nox is handling simple Auditable Data operations automatically. However for enabling this feature some steps has to be done first. Please follow these:

1. **IAuditableData interface should be added to entity class.**

<details>
<summary>Click to expand the example</summary>

```cs
using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Coupon.Domain.Entities;

public class Ticket : IAuditable // Omitted
{
    // Omitted

    #region [ IAuditable ]

    public string CreatedBy { get; } = string.Empty;

    public DateTime CreationDate { get; }

    public string? UpdatedBy { get; }

    public DateTime? UpdateDate { get; }

    #endregion

    // Omitted
}
```

</details>

<br>

2. **AuditInformation should be added to dto class.**

<details>
<summary>Click to expand the example</summary>

```cs
using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;

[TicketDetailLevel(TicketDataAccessDetailLevel.Simple)]
public class TicketSimpleDto : TicketSimpleCreateDto, IAuditDto
{
    #region [ Properties ]

    public AuditInformation AuditInformation { get; set; } = default!;

    #endregion
}
```

</details>

<br>

3. **In Dto Mapping `MapAuditInformation` should be used**

<details>
<summary>Click to expend the example</summary>

```cs
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Models.Basic;
using AppyNox.Services.Coupon.Domain.Entities;
using AutoMapper;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.Profiles;

public class TicketProfile : Profile
{
    #region [ Public Constructors ]

    public TicketProfile()
    {
        CreateMap<Ticket, TicketSimpleDto>().MapAuditInformation();
        // Omitted
    }

    #endregion
}
```

</details>

4. **During saving UnitOfWorkBase should be used.** <br>
   A UnitOfWork which inherits UnitOfWorkBase should be used.

5. **During saving isSystem should be informed** <br>
   If isSystem parameter is set to true, CreatedBy or UpdatedBy fields will be populated as `System`, if not (default is false) mentioned fields will be set to current user id.
   This information is accessed from `NoxContext.UserId`. Keep in mind that if `NoxContext.UserId` is not set, mentioned fields will be set as `Unknown`.
