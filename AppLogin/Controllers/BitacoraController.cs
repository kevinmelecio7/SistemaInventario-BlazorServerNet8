using AppLogin.DTOs.Excel;
using AppLogin.Repos;
using AppLogin.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        private readonly IBitacora bitacorarepo;
        
        public BitacoraController(IBitacora bitacorarepo)
        {
            this.bitacorarepo = bitacorarepo;
        }

        [HttpPost("InsertBitacora")]
        [AllowAnonymous]
        public async Task<ActionResult> InsertBitacora(BitacoraDTO model)
        {
            try
            {
                await bitacorarepo.InsertBitacoraAsync(model);
                var response = new ApiResponse<List<BitacoraDTO>> { Mensaje = "ok", Response = null };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<BitacoraDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
