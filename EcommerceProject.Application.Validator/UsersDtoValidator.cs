using EcommerceProject.Application.DTO;
using FluentValidation;

namespace EcommerceProject.Application.Validator
{
    public class UsersDtoValidator : AbstractValidator<UserDto>
    {
        public UsersDtoValidator() 
        {
            RuleFor(u => u.UserName).NotNull().NotEmpty();
            RuleFor(u => u.Password).NotNull().NotEmpty();
        }

    }
}