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
        List<MasterDataDTO> dtosMaster;
        List<InitialLoadDTO> dtosInitial;
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

        [HttpGet("GetMaterial")] //GetMaterial?periodo=
        [AllowAnonymous]
        public async Task<IActionResult> GetMaterial([FromQuery] int periodo)
        {

            try
            {
                dtosMaster = await inputsrepo.GetMasterDataAsync(periodo);
                var response = new ApiResponse<List<MasterDataDTO>> { Mensaje = "ok", Response = dtosMaster };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<MasterDataDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse); ;
            }
        }

        [HttpPost("InsertMaterial")]
        [AllowAnonymous]
        public async Task<ActionResult> InsertMasterData(List<MasterDataDTO> list)
        {
            try
            {
                await inputsrepo.InsertMasterDataAsync(list);
                var response = new ApiResponse<List<MasterDataDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<MasterDataDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpDelete("DeleteMaterial")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteMasterData([FromBody] List<int> ids)
        {
            try
            {
                await inputsrepo.DeleteMasterDataAsync(ids);
                var response = new ApiResponse<List<MasterDataDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<MasterDataDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("GetInitialLoad")] //GetInitialLoad?periodo=
        [AllowAnonymous]
        public async Task<IActionResult> GetInitialLoad([FromQuery] int periodo)
        {

            try
            {
                dtosInitial = await inputsrepo.GetInitialLoadAsync(periodo);
                var response = new ApiResponse<List<InitialLoadDTO>> { Mensaje = "ok", Response = dtosInitial };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<InitialLoadDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse); ;
            }
        }
        [HttpPost("InsertInitialLoad")]
        [AllowAnonymous]
        public async Task<ActionResult> InsertInitialLoad(List<InitialLoadDTO> list)
        {
            try
            {
                await inputsrepo.InsertInitialLoadAsync(list);
                var response = new ApiResponse<List<InitialLoadDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<InitialLoadDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        [HttpDelete("DeleteInitialLoad")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteInitialLoad([FromBody] List<int> ids)
        {
            try
            {
                await inputsrepo.DeleteInitialLoadAsync(ids);
                var response = new ApiResponse<List<InitialLoadDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<InitialLoadDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
