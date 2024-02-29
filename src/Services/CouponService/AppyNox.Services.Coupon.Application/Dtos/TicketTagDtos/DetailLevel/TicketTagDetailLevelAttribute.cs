using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Coupon.Application.Dtos.TicketTagDtos.DetailLevel;

public enum TicketTagDataAccessDetailLevel
{
    [Display(Name = "Simple")]
    Simple
}

public enum TicketTagCreateDetailLevel
{
    [Display(Name = "Simple")]
    Simple
}

public enum TicketTagUpdateDetailLevel
{
    [Display(Name = "Simple")]
    Simple
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class TicketTagDetailLevelAttribute : Attribute
{
    #region [ Public Constructors ]

    public TicketTagDetailLevelAttribute(TicketTagDataAccessDetailLevel dataAccessDetailLevel)
    {
        DataAccessDetailLevel = dataAccessDetailLevel;
    }

    public TicketTagDetailLevelAttribute(TicketTagCreateDetailLevel createDetailLevel)
    {
        CreateDetailLevel = createDetailLevel;
    }

    public TicketTagDetailLevelAttribute(TicketTagUpdateDetailLevel updateDetailLevel)
    {
        UpdateDetailLevel = updateDetailLevel;
    }

    #endregion

    #region [ Properties ]

    public TicketTagDataAccessDetailLevel DataAccessDetailLevel { get; }

    public TicketTagCreateDetailLevel CreateDetailLevel { get; }

    public TicketTagUpdateDetailLevel UpdateDetailLevel { get; }

    #endregion
}