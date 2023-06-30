using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Application.Interface.Persistence
{
    public interface IUsersRepository
    {
        User Authenticate(string username, string password);
    }
}
