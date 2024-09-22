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
                                    correo = reader["Email"].ToString(),
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

        public async Task UpdateUserAsync(UserDTO user)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string sql = "UPDATE Users SET Name = @name, Email = @email, Role = @role WHERE Id = @id;";

                            using (var command = new SqlCommand(sql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", user.id);
                                command.Parameters.AddWithValue("@name", user.nombre);
                                command.Parameters.AddWithValue("@email", user.correo);
                                command.Parameters.AddWithValue("@role", user.rol);

                                await command.ExecuteNonQueryAsync();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error al actualizar: ", ex);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Error SQL: ", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }
        public async Task DeleteUserAsync(UserDTO user)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string sql = "DELETE FROM Users WHERE Id = @id;";

                            using (var command = new SqlCommand(sql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", user.id);

                                await command.ExecuteNonQueryAsync();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error al eliminar: ", ex);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Error SQL: ", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }
    }
}
