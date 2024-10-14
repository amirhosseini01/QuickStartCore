using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;

namespace Server.Core.Modules.User.Dto;

public class RegisterInput
{
	[DisplayName(Messages.PhoneNumber)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MinLengthModelState)]
	public string PhoneNumber { get; set; }

	[DisplayName(Messages.Password)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.PasswordRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.PasswordMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	[DataType(DataType.Password, ErrorMessage = Messages.DataTypeModelState)]
	public string Password { get; set; }

	[DisplayName(Messages.ConfirmPassword)]
	[DataType(DataType.Password, ErrorMessage = Messages.DataTypeModelState)]
	[Compare(nameof(Password), ErrorMessage = Messages.PasswordCompareModelState)]
	public string ConfirmPassword { get; set; }
}

public class LoginInput
{
	[DisplayName(Messages.PhoneNumber)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MinLengthModelState)]
	public string PhoneNumber { get; set; }
	
	[DisplayName(Messages.Password)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[DataType(DataType.Password, ErrorMessage = Messages.DataTypeModelState)]
	[MaxLength(ModelStatics.PasswordRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.PasswordMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string Password { get; set; }

	[Display(Name = Messages.RememberMe)]
	public bool RememberMe { get; set; }
}

public class RegisterConfirmationInputDto
{
	[DisplayName(Messages.PhoneNumber)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MinLengthModelState)]
	public string PhoneNumber { get; set; }
	
	[DisplayName(Messages.VerifyCode)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.VerifyCodeRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.VerifyCodeRequiredLength, ErrorMessage = Messages.MinLengthModelState)]
	public string VerifyCode { get; set; }
}
