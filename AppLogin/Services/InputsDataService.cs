using AppLogin.DTOs;
using AppLogin.DTOs.Excel;
using AppLogin.Responses;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text;


namespace AppLogin.Services
{
    public class InputsDataService
    {
        private readonly BitacoraService bitacoraService;

        private readonly HttpClient httpClient;
        public InputsDataService(HttpClient httpClient, BitacoraService bitacoraService)
        {
            this.httpClient = httpClient;
            this.bitacoraService = bitacoraService;
        }
        private const string BaseUrl = "api/InputsData";

        public async Task<List<PeriodoDTO>> GetPeriodoAsync()
        {
            try
            {
                string apiURL = $"{BaseUrl}/GetPeriodo";
                var response = await httpClient.GetFromJsonAsync<ApiResponse<List<PeriodoDTO>>>(apiURL);
                if (response != null && response.Mensaje == "ok")
                {
                    return response.Response ?? new List<PeriodoDTO>();
                }
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "GetPeriodoAsync", tipo = "ERROR", descripcion = response.ToString(), usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return new List<PeriodoDTO>();
            }
            catch (Exception ex)
            {
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "GetPeriodoAsync", tipo = "ERROR", descripcion = ex.Message, usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return new List<PeriodoDTO>();
            }
        }

        public async Task <string>InsertPeriodoAsync(PeriodoDTO model)
        {
            try
            {
                string apiURL = $"{BaseUrl}/InsertPeriodo";
                var response = await httpClient.PostAsJsonAsync(apiURL, model);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
                    if (apiResponse!.Response == null && apiResponse!.Mensaje == "ok")
                    {
                        return apiResponse.Mensaje;
                    }
                }
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "InsertPeriodoAsync", tipo = "ERROR", descripcion = response.ToString(), usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
            catch (Exception ex)
            {
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "InsertPeriodoAsync", tipo = "ERROR", descripcion = ex.Message, usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
        }

        public async Task<string> UpdatePeriodoAsync(PeriodoDTO model)
        {
            try
            {
                string apiURL = $"{BaseUrl}/UpdatePeriodo";
                var response = await httpClient.PutAsJsonAsync(apiURL, model);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
                    if (apiResponse!.Response == null && apiResponse!.Mensaje == "ok")
                    {
                        return apiResponse.Mensaje;
                    }
                }
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "UpdatePeriodoAsync", tipo = "ERROR", descripcion = response.ToString(), usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
            catch (Exception ex)
            {
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "UpdatePeriodoAsync", tipo = "ERROR", descripcion = ex.Message, usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
        }


        public async Task<List<StorageBinDTO>> GetStorageAsync(int periodo)
        {
            try
            {
                string apiURL = $"{BaseUrl}/GetStorage?periodo={periodo}";
                var response = await httpClient.GetFromJsonAsync<ApiResponse<List<StorageBinDTO>>>(apiURL);
                if (response != null && response.Mensaje == "ok")
                {
                    return response.Response ?? new List<StorageBinDTO>();
                }
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "GetStorageAsync", tipo = "ERROR", descripcion = response.ToString(), usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return new List<StorageBinDTO>();
            }
            catch (Exception ex)
            {
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "GetStorageAsync", tipo = "ERROR", descripcion = ex.Message, usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return new List<StorageBinDTO>();
            }
        }

        public async Task<string> InsertStorageAsync(List<StorageBinDTO> list)
        {
            try
            {
                string apiURL = $"{BaseUrl}/InsertStorage";
                var response = await httpClient.PostAsJsonAsync(apiURL, list);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
                    if (apiResponse!.Response == null && apiResponse!.Mensaje == "ok")
                    {
                        return apiResponse.Mensaje;
                    }
                }
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "InsertStorageAsync", tipo = "ERROR", descripcion = response.ToString(), usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
            catch (Exception ex)
            {
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsDataService", accion = "InsertStorageAsync", tipo = "ERROR", descripcion = ex.Message, usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
        }

        public async Task<string> DeleteStorageAsync(List<StorageBinDTO> list)
        {
            try
            {
                var ids =  list.Select(x => x.id);
                //var jsonBody = JsonSerializer.Serialize(ids);
                //Console.WriteLine(jsonBody);
                var content = new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json");

                string apiURL = $"{BaseUrl}/DeleteStorage";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiURL, UriKind.RelativeOrAbsolute), // Ruta relativa o absoluta según sea necesario
                    Content = content
                };
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
                    if (apiResponse!.Response == null && apiResponse!.Mensaje == "ok") 
                    {
                        return apiResponse.Mensaje;
                    }
                }
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsData", accion = "DeleteStorageAsync", tipo = "ERROR", descripcion = response.ToString(), usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
            catch (Exception ex)
            {
                BitacoraDTO bitacora = new BitacoraDTO { vista = "InputsData", accion = "DeleteStorageAsync", tipo = "ERROR", descripcion = ex.Message, usuario = "0" };
                await bitacoraService.InsertBitacoraAsync(bitacora);
                return "Error";
            }
        }
    }
}
