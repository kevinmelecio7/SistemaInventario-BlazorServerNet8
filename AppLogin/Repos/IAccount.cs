using AppLogin.DTOs;
using static AppLogin.Responses.CustomResponses;

namespace AppLogin.Repos
{
    public interface IAccount
    {
        Task<RegistrationResponse> RegisterAsync(RegisterDTO model);
        Task<LoginResponse> LoginAsync(LoginDTO model);
        Task<LoginResponse> LogoutAsync();
    }
}
