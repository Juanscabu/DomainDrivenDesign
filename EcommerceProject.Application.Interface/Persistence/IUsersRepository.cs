using EcommerceProject.Domain.Entities;

namespace EcommerceProject.Application.Interface.Persistence
{
    public interface IUsersRepository
    {
        User Authenticate(string username, string password);
    }
}
