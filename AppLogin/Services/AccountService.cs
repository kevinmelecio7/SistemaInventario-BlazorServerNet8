using AppLogin.DTOs;
using AppLogin.States;
using AppLogin.Components.Pages;
using System.Net.Http;
using System.Text;
using static AppLogin.Responses.CustomResponses;

namespace AppLogin.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;
        public AccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private const string BaseUrl = "api/account";

        public async Task<WeatherForecast[]> GetWeatherForecasts()
        {
            var response = await httpClient.GetAsync($"{BaseUrl}/weather");
            bool check = CheckIfUnauthorized(response);
            if (check) return null!;
            if (Constants.JWTToken == "") return null!;
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.JWTToken);
            return await httpClient.GetFromJsonAsync<WeatherForecast[]>($"{BaseUrl}/weather");
        }

        private static bool CheckIfUnauthorized(HttpResponseMessage httpResponseMessage)
        {
            if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return true;
            } else return false;
        }

        public async Task<LoginResponse> LoginAsync(LoginDTO model)
        {
            var response = await httpClient.PostAsJsonAsync($"{BaseUrl}/login", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            else
            {
                // Puedes leer el contenido como texto para ver el mensaje de error
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error en la solicitud: {errorMessage}");
            }
            //var response = await httpClient.PostAsJsonAsync($"{BaseUrl}/login", model);
            //var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            //return result!;
        }

        public async Task<RegistrationResponse> RegisterAsync(RegisterDTO model)
        {
            var response = await httpClient.PostAsJsonAsync($"{BaseUrl}/register", model);
            var result = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
            return result!;
        }

        public async Task<LoginResponse> LogoutAsync()
        {
            var response = await httpClient.PostAsync($"{BaseUrl}/logout", null);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result!;
        }
    }
}
