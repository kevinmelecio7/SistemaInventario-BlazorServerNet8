using AppLogin.DTOs;

namespace AppLogin.Repos
{
    public interface IUser
    {
        Task<List<UserDTO>> GetUsersAsync();
    }
}
