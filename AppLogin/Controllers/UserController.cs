using AppLogin.DTOs;
using AppLogin.Repos;
using AppLogin.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AppLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser userrepo;
        public UserController(IUser userrepo)
        {
            this.userrepo = userrepo;
        }

        [HttpGet("GetUsers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers()
        {
            List<UserDTO> dtos;
            try
            {
                dtos = await userrepo.GetUsersAsync();
                var response = new ApiResponse<List<UserDTO>> { Mensaje = "ok", Response = dtos };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<UserDTO>> { Mensaje = ex.Message, Response = null };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse); ;
            }
        }
    }

    
}
