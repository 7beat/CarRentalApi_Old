using FluentValidation;

namespace CarRentalAPI.Validators
{
    public class LoginValidator : AbstractValidator<Models.DTO.LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
