namespace AppyNox.Services.Base.Core.Constants;

public static class LocalizationConstants
{
    public static readonly LanguageCode[] SupportedLanguages =
    {
        new()
        {
            Code = "en-US",
            DisplayName = "English"
        },
        new()
        {
            Code = "tr-TR",
            DisplayName = "Turkish"
        }
    };
}

public class LanguageCode
{
    public string DisplayName { get; set; } = "en-US";
    public string Code { get; set; } = "English";
}