using AppLogin.DTOs;
using AppLogin.DTOs.Excel;
using AppLogin.Repos;
using AppLogin.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Asn1.X509;

namespace AppLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputsDataController : ControllerBase
    {
        private readonly IInputsData inputsrepo;
        List<PeriodoDTO> dtosPeriodo;
        List<StorageBinDTO> dtosStorage;
        public InputsDataController(IInputsData inputsrepo)
        {
            this.inputsrepo = inputsrepo;
        }

        [HttpGet("GetPeriodo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPeriodo()
        {
            
            try
            {
                dtosPeriodo = await inputsrepo.GetPeriodoAsync();
                var response = new ApiResponse<List<PeriodoDTO>> { Mensaje = "ok", Response = dtosPeriodo };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<PeriodoDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse); ;
            }
        }

        [HttpPost("InsertPeriodo")]
        [AllowAnonymous]
        public async Task<ActionResult> InsertPeriodo(PeriodoDTO model)
        {
            try
            {
                await inputsrepo.InsertPeriodoAsync(model);
                var response = new ApiResponse<List<PeriodoDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<PeriodoDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut("UpdatePeriodo")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdatePeriodo(PeriodoDTO model)
        {
            try
            {
                await inputsrepo.UpdatePeriodoAsync(model);
                var response = new ApiResponse<List<PeriodoDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<PeriodoDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("GetStorage")] //GetStorage?periodo=
        [AllowAnonymous]
        public async Task<IActionResult> GetStorage([FromQuery] int periodo)
        {

            try
            {
                dtosStorage = await inputsrepo.GetStorageAsync(periodo);
                var response = new ApiResponse<List<StorageBinDTO>> { Mensaje = "ok", Response = dtosStorage };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<StorageBinDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse); ;
            }
        }

        [HttpPost("InsertStorage")]
        [AllowAnonymous]
        public async Task<ActionResult> InsertStorage(List<StorageBinDTO> list)
        {
            try
            {
                await inputsrepo.InsertStorageAsync(list);
                var response = new ApiResponse<List<StorageBinDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<StorageBinDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpDelete("DeleteStorage")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteStorage([FromBody] List<int> ids)
        {
            try
            {
                await inputsrepo.DeleteStorageAsync(ids);
                var response = new ApiResponse<List<StorageBinDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<StorageBinDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
