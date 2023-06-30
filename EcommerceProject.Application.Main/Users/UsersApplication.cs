using AutoMapper;
using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Interface.Features;
using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Application.Validator;
using EcommerceProject.Transversal.Common;

namespace EcommerceProject.Application.Feature.Users
{
    public class UsersApplication : IUsersApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UsersDtoValidator _usersDtoValidator;

        public UsersApplication(IUnitOfWork unitOfWork, IMapper mapper, UsersDtoValidator usersDtoValidator)
        {
            _unitOfWork = unitOfWork;
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
                var user = _unitOfWork.Users.Authenticate(username, password);
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
