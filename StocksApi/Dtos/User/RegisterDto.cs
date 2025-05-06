using System.ComponentModel.DataAnnotations;

namespace StocksApi.Dtos.User
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        //[DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
