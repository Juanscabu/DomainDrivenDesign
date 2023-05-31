using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Domain.Interface
{
    public interface IUsersDomain
    {
        User Authenticate(string username, string password);
    }
}
