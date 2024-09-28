using AppLogin.DTOs;

namespace AppLogin.Repos
{
    public interface IUser
    {
        Task<List<UserDTO>> GetUsersAsync();
        Task UpdateUserAsync(UserDTO user);
        Task UpdatePasswordUserAsync(UserDTO user);
        Task DeleteUserAsync(UserDTO user);
        
    }
}
