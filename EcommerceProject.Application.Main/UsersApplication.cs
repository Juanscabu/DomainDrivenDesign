using AutoMapper;
using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Interface;
using EcommerceProject.Domain.Entity;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Transversal.Common;

namespace EcommerceProject.Application.Main
{
    public class UsersApplication : IUsersApplication
    {
        private readonly IUsersDomain _userDomain;
        private readonly IMapper _mapper;

        public UsersApplication(IUsersDomain userDomain, IMapper mapper)
        {
            _userDomain = userDomain;
            _mapper = mapper;
        }

        public Response<UserDto> Authenticate(string username, string password)
        {
            var response = new Response<UserDto>();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                response.Message = "Parameters can not be null";
                return response;
            }

            try
            {
                var user = _userDomain.Authenticate(username, password);
                response.Data = _mapper.Map<UserDto>(user);
                response.IsSuccess = true;
                response.Message = "Succesfull Authentication";
            }
            catch (InvalidOperationException)
            {
                response.IsSuccess = true;
                response.Message = "Invalid Username";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }
            return response;
        }
    }
}
