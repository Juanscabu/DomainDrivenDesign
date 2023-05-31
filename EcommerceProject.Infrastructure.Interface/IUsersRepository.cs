using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Infrastructure.Interface
{
    public interface IUsersRepository
    {
        User Authenticate(string username, string password);
    }
}
