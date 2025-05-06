using StocksApi.Models;

namespace StocksApi.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
