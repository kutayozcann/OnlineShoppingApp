using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models;

public class RegisterRequest
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string PhoneNumber { get; set; }
}