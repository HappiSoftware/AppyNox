using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketDtos.DetailLevel;

public enum TicketDataAccessDetailLevel
{
    [Display(Name = "Simple")]
    Simple,

    [Display(Name = "Extended")]
    Extended,
}

public enum TicketCreateDetailLevel
{
    [Display(Name = "Simple")]
    Simple
}

public enum TicketUpdateDetailLevel
{
    [Display(Name = "Simple")]
    Simple
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class TicketDetailLevelAttribute : Attribute
{
    #region [ Public Constructors ]

    public TicketDetailLevelAttribute(TicketDataAccessDetailLevel dataAccessDetailLevel)
    {
        DataAccessDetailLevel = dataAccessDetailLevel;
    }

    public TicketDetailLevelAttribute(TicketCreateDetailLevel createDetailLevel)
    {
        CreateDetailLevel = createDetailLevel;
    }

    public TicketDetailLevelAttribute(TicketUpdateDetailLevel updateDetailLevel)
    {
        UpdateDetailLevel = updateDetailLevel;
    }

    #endregion

    #region [ Properties ]

    public TicketDataAccessDetailLevel DataAccessDetailLevel { get; }

    public TicketCreateDetailLevel CreateDetailLevel { get; }

    public TicketUpdateDetailLevel UpdateDetailLevel { get; }

    #endregion
}