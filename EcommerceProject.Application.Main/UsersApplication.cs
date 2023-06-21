using AutoMapper;
using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Interface;
using EcommerceProject.Application.Validator;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Transversal.Common;

namespace EcommerceProject.Application.Main
{
    public class UsersApplication : IUsersApplication
    {
        private readonly IUsersDomain _userDomain;
        private readonly IMapper _mapper;
        private readonly UsersDtoValidator _usersDtoValidator;

        public UsersApplication(IUsersDomain userDomain, IMapper mapper, UsersDtoValidator usersDtoValidator)
        {
            _userDomain = userDomain;
            _mapper = mapper;
            _usersDtoValidator = usersDtoValidator;
        }

        public Response<UserDto> Authenticate(string username, string password)
        {
            var response = new Response<UserDto>();
            var validation = _usersDtoValidator.Validate(new UserDto() { UserName = username, Password = password });
            if (!validation.IsValid)
            {
                response.Message = "Parameters can not be null";
                response.Errors = validation.Errors;
                return response;
            }

            try
            {
                var user = _userDomain.Authenticate(username, password);
                response.Data = _mapper.Map<UserDto>(user);
                response.IsSuccess = true;
                response.Message = "Successfull Authentication";
            }
            catch (InvalidOperationException)
            {
                response.IsSuccess = true;
                response.Message = "Invalid Username or Password";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }
            return response;
        }
    }
}
