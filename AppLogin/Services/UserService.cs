using AppLogin.DTOs;
using AppLogin.Responses;


namespace AppLogin.Services
{
    public class UserService
    {
        private readonly HttpClient httpClient;
        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        private const string BaseUrl = "api/User";

        public async Task<List<UserDTO>> GetUserAsync()
        {
            try
            {
                string apiURL = $"{BaseUrl}/GetUsers";
                var response = await httpClient.GetFromJsonAsync<ApiResponse<List<UserDTO>>>(apiURL);
                if (response != null && response.Mensaje == "ok")
                {
                    return response.Response ?? new List<UserDTO>();
                }
                return new List<UserDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                return new List<UserDTO>();
            }
            
        }
    }
}
