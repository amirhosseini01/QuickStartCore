using Microsoft.AspNetCore.Identity;

namespace Server.Areas.Identity;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
	public override IdentityError DefaultError()
	{
		return new IdentityError { Code = nameof(DefaultError), Description = $"خطای ناشناخته رخ داده است." };
	}

	public override IdentityError ConcurrencyFailure()
	{
		return new IdentityError
		{
			Code = nameof(ConcurrencyFailure), Description = "همزمانی رخ داده است. لطفا مجددا تلاش فرمایید."
		};
	}

	public override IdentityError PasswordMismatch()
	{
		return new IdentityError { Code = nameof(PasswordMismatch), Description = "کلمه عبور معتبر نمیباشد." };
	}

	public override IdentityError InvalidToken()
	{
		return new IdentityError { Code = nameof(InvalidToken), Description = "توکن معتبر نیست." };
	}

	public override IdentityError LoginAlreadyAssociated()
	{
		return new IdentityError
		{
			Code = nameof(LoginAlreadyAssociated), Description = "A user with this login already exists."
		};
	}

	public override IdentityError InvalidUserName(string userName)
	{
		return new IdentityError
		{
			Code = nameof(InvalidUserName),
			Description = $"نام کاربری {userName} معتبر نیست، باید شامل اعداد و حروف انگلیسی باشد."
		};
	}

	public override IdentityError InvalidEmail(string email)
	{
		return new IdentityError { Code = nameof(InvalidEmail), Description = $"ایمیل {email} معتبر نمیباشد." };
	}

	public override IdentityError DuplicateUserName(string userName)
	{
		return new IdentityError
		{
			Code = nameof(DuplicateUserName), Description = $"نام کاربری {userName} توسط شخص دیگری انتخاب شده است."
		};
	}

	public override IdentityError DuplicateEmail(string email)
	{
		return new IdentityError
		{
			Code = nameof(DuplicateEmail), Description = $"ایمیل {email} توسط شخص دیگری انتخاب شده است."
		};
	}

	public override IdentityError InvalidRoleName(string role)
	{
		return new IdentityError { Code = nameof(InvalidRoleName), Description = $"نقش {role} معتبر نمیباشد." };
	}

	public override IdentityError DuplicateRoleName(string role)
	{
		return new IdentityError
		{
			Code = nameof(DuplicateRoleName), Description = $"Role name '{role}' is already taken."
		};
	}

	public override IdentityError UserAlreadyHasPassword()
	{
		return new IdentityError
		{
			Code = nameof(UserAlreadyHasPassword), Description = "قبلا برای کاربر کلمه عبور مشخص شده است."
		};
	}

	public override IdentityError UserLockoutNotEnabled()
	{
		return new IdentityError
		{
			Code = nameof(UserLockoutNotEnabled), Description = "Lockout is not enabled for this user."
		};
	}

	public override IdentityError UserAlreadyInRole(string role)
	{
		return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"User already in role '{role}'." };
	}

	public override IdentityError UserNotInRole(string role)
	{
		return new IdentityError { Code = nameof(UserNotInRole), Description = $"User is not in role '{role}'." };
	}

	public override IdentityError PasswordTooShort(int length)
	{
		return new IdentityError
		{
			Code = nameof(PasswordTooShort), Description = $"کلمه عبور باید {length} کاراکتر داشته باشد."
		};
	}

	public override IdentityError PasswordRequiresNonAlphanumeric()
	{
		return new IdentityError
		{
			Code = nameof(PasswordRequiresNonAlphanumeric),
			Description = "Passwords must have at least one non alphanumeric character."
		};
	}

	public override IdentityError PasswordRequiresDigit()
	{
		return new IdentityError
		{
			Code = nameof(PasswordRequiresDigit), Description = "کلمه عبور باید شامل اعداد باشد ('0'-'9')."
		};
	}

	public override IdentityError PasswordRequiresLower()
	{
		return new IdentityError
		{
			Code = nameof(PasswordRequiresLower),
			Description = "Passwords must have at least one lowercase ('a'-'z')."
		};
	}

	public override IdentityError PasswordRequiresUpper()
	{
		return new IdentityError
		{
			Code = nameof(PasswordRequiresUpper),
			Description = "Passwords must have at least one uppercase ('A'-'Z')."
		};
	}
}
