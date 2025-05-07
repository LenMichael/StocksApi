using Microsoft.AspNetCore.Identity;

namespace StocksApi.Models
{
    public class User : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
