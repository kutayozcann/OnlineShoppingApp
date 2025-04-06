using OnlineShoppingApp.Data.Enums;

namespace OnlineShoppingApp.Business.Operations.User.Dtos;

public class AddUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
}