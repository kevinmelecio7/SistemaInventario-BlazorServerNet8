using AppLogin.DTOs;
using AppLogin.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AppLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string? connectionSQL;
        public UserController(IConfiguration conf)
        {
            connectionSQL = conf.GetConnectionString("DefaultConnection");
        }

        [HttpPost("GetUsers")]
        [AllowAnonymous]
        public IActionResult GetUsers()
        {
            List<UserDTO> dtos = new List<UserDTO>();
            try
            {
                using (var conexion = new SqlConnection(connectionSQL))
                {
                    conexion.Open();
                    string sql = "SELECT * FROM users;";
                    var cmd = new SqlCommand(sql, conexion);
                    cmd.ExecuteNonQuery();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            dtos.Add(new UserDTO()
                            {
                                id = Convert.ToInt32(rd["Id"]),
                                nombre = rd["Name"].ToString(),
                                noEmpleado = rd["Email"].ToString(),
                                rol = rd["Role"].ToString(),
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", respose = dtos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, respose = dtos });
            }

        }
    }

    
}
