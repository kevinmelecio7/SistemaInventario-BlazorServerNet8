using AppLogin.DTOs;
using Microsoft.Data.SqlClient;

namespace AppLogin.Repos
{
    public class User :IUser
    {
        private readonly string? _connectionSQL;

        public User(IConfiguration configuration)
        {
            _connectionSQL = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var dtos = new List<UserDTO>();

            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM users;";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dtos.Add(new UserDTO()
                                {
                                    id = Convert.ToInt32(reader["Id"]),
                                    nombre = reader["Name"].ToString(),
                                    noEmpleado = reader["Email"].ToString(),
                                    rol = reader["Role"].ToString(),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción, posiblemente registrarla
                throw new Exception("Error al obtener los usuarios", ex);
            }

            return dtos;
        }
    }
}
