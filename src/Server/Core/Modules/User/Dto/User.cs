using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;

namespace Server.Core.Modules.User.Dto;

public class UserFilter
{
	[DisplayName(Messages.EmailConfirmed)]
	public bool? EmailConfirmed { get; set; }
	
	[DisplayName(Messages.PhoneNumberConfirmed)]
	public bool? PhoneNumberConfirmed { get; set; }
}

public class UserInput
{
	[DisplayName(Messages.UserName)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string UserName { get; set; }
	
	[DisplayName(Messages.Email)]
	[DataType(DataType.EmailAddress, ErrorMessage = Messages.DataTypeModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string Email { get; set; }
	
	[DisplayName(Messages.EmailConfirmed)]
	public bool EmailConfirmed { get; set; }
	
	[DisplayName(Messages.PhoneNumber)]
	[MaxLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.PhoneNumberRequiredLength, ErrorMessage = Messages.MinLengthModelState)]
	public string PhoneNumber { get; set; }
	
	[DisplayName(Messages.PhoneNumberConfirmed)]
	public bool PhoneNumberConfirmed { get; set; }
	
	[DisplayName(Messages.Password)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[DataType(DataType.Password, ErrorMessage = Messages.DataTypeModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string PasswordHash { get; set; }
}

public class UserInputUpdate : UserInput
{
	[DisplayName(Messages.Password)]
	[DataType(DataType.Password, ErrorMessage = Messages.DataTypeModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string PasswordHash { get; set; }
}