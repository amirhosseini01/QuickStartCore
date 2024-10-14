using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Server.Core.Commons;

public static class ModelStatics
{
    public const int TitleRequiredLength = 250;
    public const int TitleMinimumLength = 2;
    public const int VerifyCodeRequiredLength = 5;
    public const int PasswordRequiredLength = 100;
    public const int PasswordMinimumLength = 6;
    public const int ImageRequiredLength = 500;
    public const int UrlRequiredLength = 500;
    public const int UrlMinimumLength = 2;
    public const int DescriptionRequiredLength = 500;
    public const int DescriptionMinimumLength = 5;
    public const int PhoneNumberRequiredLength = 11;
    public const string Nvarchar50 = "nvarchar(50)";
    public const int MaximumIdRange = int.MaxValue;
    public const int MinimumIdRange = 1;

    public static bool IsNotValid(this ModelStateDictionary modelState)
    {
        return !modelState.IsValid;
    }

    public static List<string> GetModeStateErrors(this ModelStateDictionary modelState)
    {
        return modelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList();
    }
}
