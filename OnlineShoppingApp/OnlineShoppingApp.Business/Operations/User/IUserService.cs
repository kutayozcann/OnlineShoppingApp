using OnlineShoppingApp.Business.Operations.User.Dtos;
using OnlineShoppingApp.Business.Types;

namespace OnlineShoppingApp.Business.Operations.User;

public interface IUserService
{
    Task<ServiceMessage> AddUser(AddUserDto user);
    ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
}