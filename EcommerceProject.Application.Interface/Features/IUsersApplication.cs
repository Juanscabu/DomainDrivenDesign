using EcommerceProject.Application.DTO;
using EcommerceProject.Transversal.Common;

namespace EcommerceProject.Application.Interface.Features
{
    public interface IUsersApplication
    {
        Response<UserDto> Authenticate(string username, string password);
    }
}
