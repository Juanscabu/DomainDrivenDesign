using System.Data;

namespace EcommerceProject.Transversal.Common
{
    public interface IConnectionFactory
    {
        IDbConnection? GetConnection { get; }
    }
}
