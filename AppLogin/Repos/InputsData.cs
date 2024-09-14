using AppLogin.DTOs;
using AppLogin.DTOs.Excel;
using Microsoft.Data.SqlClient;

namespace AppLogin.Repos
{
    public class InputsData : IInputsData
    {
        private readonly string? _connectionSQL;
        public InputsData(IConfiguration configuration)
        {
            _connectionSQL = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<PeriodoDTO>> GetPeriodoAsync()
        {
            var dtos = new List<PeriodoDTO>();
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM periodo ORDER BY id_periodo desc;";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dtos.Add(new PeriodoDTO
                                {
                                    id = Convert.ToInt32(reader["id_periodo"]),
                                    periodo = reader["periodo"].ToString(),
                                    activo = Convert.ToInt32(reader["activo"]),
                                    fecha = DateTime.Parse(reader["fecha"].ToString()),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
            return dtos;
        }

        public async Task InsertPeriodoAsync(PeriodoDTO model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "INSERT INTO periodo (periodo, activo, fecha) VALUES (@Periodo, @Activo, @Fecha)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Periodo", model.periodo);
                        command.Parameters.AddWithValue("@Activo", model.activo);
                        command.Parameters.AddWithValue("@Fecha", model.fecha);

                        await command.ExecuteNonQueryAsync();
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

        public async Task UpdatePeriodoAsync(PeriodoDTO model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "UPDATE periodo SET periodo = @Periodo, activo = @Activo, fecha = @Fecha WHERE id_periodo = @Id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.id);
                        command.Parameters.AddWithValue("@Periodo", model.periodo);
                        command.Parameters.AddWithValue("@Activo", model.activo);
                        command.Parameters.AddWithValue("@Fecha", model.fecha);

                        await command.ExecuteNonQueryAsync();
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


        public async Task<List<StorageBinDTO>> GetStorageAsync( int periodo)
        {
            var dtos = new List<StorageBinDTO>();
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM data_storage WHERE fkPeriodo = @Periodo;";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Periodo", periodo);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dtos.Add(new StorageBinDTO
                                {
                                    id = Convert.ToInt32(reader["id_storage"]),
                                    storagebin = reader["storagebin"].ToString(),
                                    fkPeriodo = Convert.ToInt32(reader["fkPeriodo"]),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
            return dtos;
        }

        public async Task InsertStorageAsync(List<StorageBinDTO> list)
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
                            string sql = "INSERT INTO data_storage (storagebin, fkPeriodo) VALUES (@storagebin, @fkperiodo);";
                            foreach(var obj in list)
                            {
                                using (var command = new SqlCommand(sql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@storagebin", obj.storagebin);
                                    command.Parameters.AddWithValue("@fkperiodo", obj.fkPeriodo);

                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            transaction.Commit();
                        } catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error al insertar: ", ex);
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

        public async Task DeleteStorageAsync(List<int> list)
        {
            string sql = string.Empty;
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            sql = "DELETE FROM data_storage WHERE id_storage = @id;";
                            foreach (var obj in list)
                            {
                                using (var command = new SqlCommand(sql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@id", obj);

                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error al eliminar: ", ex);
                        }
                    }
                    sql = "DECLARE @ultimoId INT; " +
                        "SELECT @ultimoId = ISNULL(MAX(id_storage), 0) FROM data_storage;" +
                        "DBCC CHECKIDENT ('data_storage', RESEED, @ultimoId);";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        await command.ExecuteNonQueryAsync();
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
