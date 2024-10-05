using AppLogin.DTOs;
using AppLogin.DTOs.Excel;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

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
        public async Task UpdateStorageAsync(StorageBinDTO obj)
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
                            string sql = "UPDATE data_storage SET storagebin = @storagebin, fkPeriodo = @fkperiodo WHERE id_storage = @id_storage;";

                            using (var command = new SqlCommand(sql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id_storage", obj.id);
                                command.Parameters.AddWithValue("@storagebin", obj.storagebin);
                                command.Parameters.AddWithValue("@fkperiodo", obj.fkPeriodo);

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


        public async Task<List<MasterDataDTO>> GetMasterDataAsync(int periodo)
        {
            var dtos = new List<MasterDataDTO>();
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM data_materials WHERE fkPeriodo = @Periodo;";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Periodo", periodo);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dtos.Add(new MasterDataDTO
                                {
                                    id_material = Convert.ToInt32(reader["id_material"]),
                                    materialID = reader["materialID"].ToString(),
                                    descripcion = reader["descripcion"].ToString(),
                                    unit_price = double.Parse(reader["unit_price"].ToString()),
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
        public async Task InsertMasterDataAsync(List<MasterDataDTO> list)
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
                            string sql = "INSERT INTO data_materials (materialID, descripcion, unit_price, fkPeriodo) VALUES (@materialID, @descripcion, @unit_price, @fkperiodo);";
                            foreach (var obj in list)
                            {
                                using (var command = new SqlCommand(sql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@materialID", obj.materialID);
                                    command.Parameters.AddWithValue("@descripcion", obj.descripcion);
                                    command.Parameters.AddWithValue("@unit_price", obj.unit_price);
                                    command.Parameters.AddWithValue("@fkperiodo", obj.fkPeriodo);

                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
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
        public async Task UpdateMasterDataAsync(MasterDataDTO obj)
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
                            string sql = "UPDATE data_materials SET materialID = @materialID, descripcion = @descripcion, unit_price = @unit_price, fkPeriodo = @fkperiodo WHERE id_material = @id_material;";

                            using (var command = new SqlCommand(sql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id_material", obj.id_material);
                                command.Parameters.AddWithValue("@materialID", obj.materialID);
                                command.Parameters.AddWithValue("@descripcion", obj.descripcion);
                                command.Parameters.AddWithValue("@unit_price", obj.unit_price);
                                command.Parameters.AddWithValue("@fkperiodo", obj.fkPeriodo);

                                await command.ExecuteNonQueryAsync();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
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
        public async Task DeleteMasterDataAsync(List<int> list)
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
                            sql = "DELETE FROM data_materials WHERE id_material = @id;";
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
                        "SELECT @ultimoId = ISNULL(MAX(id_material), 0) FROM data_materials;" +
                        "DBCC CHECKIDENT ('data_materials', RESEED, @ultimoId);";
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


        public async Task<List<InitialLoadDTO>> GetInitialLoadAsync(int periodo)
        {
            var dtos = new List<InitialLoadDTO>();
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM saldos_iniciales WHERE fkPeriodo = @Periodo;";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Periodo", periodo);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dtos.Add(new InitialLoadDTO
                                {
                                    id = Convert.ToInt32(reader["id_saldos"]),
                                    plant = reader["plant"].ToString(),
                                    warehouse = reader["warehouse"].ToString(),
                                    storage_location = reader["storage_location"].ToString(),
                                    storage_type = reader["storage_type"].ToString(),
                                    storage_bin = reader["storage_bin"].ToString(),
                                    storage_unit = reader["storage_unit"].ToString(),
                                    material_number = reader["material_number"].ToString(),
                                    material_description = reader["material_description"].ToString(),
                                    base_unit_of_measure = reader["base_unit_of_measure"].ToString(),
                                    total_quantity = double.Parse(reader["total_quantity"].ToString()),
                                    total_cost = double.Parse(reader["total_cost"].ToString()),
                                    currency = reader["currency"].ToString(),
                                    unit_standard_cost = double.Parse(reader["unit_standard_cost"].ToString()),
                                    unrestricted_stock = double.Parse(reader["unrestricted_stock"].ToString()),
                                    blocked_stock = double.Parse(reader["blocked_stock"].ToString()),
                                    quality_inspection = double.Parse(reader["quality_inspection"].ToString()),
                                    returns_stock = double.Parse(reader["returns_stock"].ToString()),
                                    transfer_stock = double.Parse(reader["transfer_stock"].ToString()),
                                    consignment_stock = double.Parse(reader["consignment_stock"].ToString()),
                                    consignment_value = double.Parse(reader["consignment_value"].ToString()),
                                    execution_date = reader["execution_date"].ToString(),
                                    fkPeriodo = Convert.ToInt32(reader["fkPeriodo"]),
                                    folio = reader["folio"].ToString(),
                                    estado = reader["estado"].ToString(),
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
        public async Task InsertInitialLoadAsync(List<InitialLoadDTO> list)
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
                            string sql = "INSERT INTO saldos_iniciales( plant, warehouse, storage_location, storage_type, storage_bin, storage_unit, material_number, material_description, base_unit_of_measure, total_quantity, total_cost, currency, unit_standard_cost, unrestricted_stock, blocked_stock, quality_inspection, returns_stock, transfer_stock,  consignment_stock, consignment_value, execution_date, fkPeriodo) " +
                                "VALUES(@plant, @warehouse, @storage_location, @storage_type, @storage_bin, @storage_unit, @material_number, @material_description, @base_unit_of_measure, @total_quantity, @total_cost, @currency, @unit_standard_cost, @unrestricted_stock, @blocked_stock, @quality_inspection, @returns_stock, @transfer_stock,  @consignment_stock, @consignment_value, @execution_date, @fkPeriodo)";
                            foreach (var obj in list)
                            {
                                using (var command = new SqlCommand(sql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@plant", obj.plant);
                                    command.Parameters.AddWithValue("@warehouse", obj.warehouse);
                                    command.Parameters.AddWithValue("@storage_location", obj.storage_location);
                                    command.Parameters.AddWithValue("@storage_type", obj.storage_type);
                                    command.Parameters.AddWithValue("@storage_bin", obj.storage_bin);
                                    command.Parameters.AddWithValue("@storage_unit", obj.storage_unit);
                                    command.Parameters.AddWithValue("@material_number", obj.material_number);
                                    command.Parameters.AddWithValue("@material_description", obj.material_description);
                                    command.Parameters.AddWithValue("@base_unit_of_measure", obj.base_unit_of_measure);
                                    command.Parameters.AddWithValue("@total_quantity", obj.total_quantity);
                                    command.Parameters.AddWithValue("@total_cost", obj.total_cost);
                                    command.Parameters.AddWithValue("@currency", obj.currency);
                                    command.Parameters.AddWithValue("@unit_standard_cost", obj.unit_standard_cost);
                                    command.Parameters.AddWithValue("@unrestricted_stock", obj.unrestricted_stock);
                                    command.Parameters.AddWithValue("@blocked_stock", obj.blocked_stock);
                                    command.Parameters.AddWithValue("@quality_inspection", obj.quality_inspection);
                                    command.Parameters.AddWithValue("@returns_stock", obj.returns_stock);
                                    command.Parameters.AddWithValue("@transfer_stock", obj.transfer_stock);
                                    command.Parameters.AddWithValue("@consignment_stock", obj.consignment_stock);
                                    command.Parameters.AddWithValue("@consignment_value", obj.consignment_value);
                                    command.Parameters.AddWithValue("@execution_date", obj.execution_date);
                                    command.Parameters.AddWithValue("@fkPeriodo", obj.fkPeriodo);
                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
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
        public async Task DeleteInitialLoadAsync(List<int> list)
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
                            sql = "DELETE FROM saldos_iniciales WHERE id_saldos = @id;";
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
                        "SELECT @ultimoId = ISNULL(MAX(id_saldos), 0) FROM saldos_iniciales;" +
                        "DBCC CHECKIDENT ('saldos_iniciales', RESEED, @ultimoId);";
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
        public async Task UpdateInitialLoadFolioAsync()
        {
            List<InitialLoadDTO> listInitial = new List<InitialLoadDTO>();
            List<PeriodoDTO> listPeriodo = new List<PeriodoDTO>();
            
            listPeriodo = await GetPeriodoAsync();
            var periodoActual = listPeriodo.Where(item => item.activo == 1).FirstOrDefault();
            string stringPeriodo = periodoActual!.periodo!.Replace("-", "");
            listInitial = await GetInitialLoadAsync(periodoActual.id);
            string folio = string.Empty;
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
                            int i = 0;
                            sql = "UPDATE saldos_iniciales SET folio = @folio, estado = @estado WHERE id_saldos = @id;";
                            foreach (var obj in listInitial)
                            {
                                i++;
                                folio = "R" + stringPeriodo + "-" + i;
                                //Console.WriteLine(folio + "\n");
                                using (var command = new SqlCommand(sql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@id", obj.id);
                                    command.Parameters.AddWithValue("@folio", folio);
                                    command.Parameters.AddWithValue("@estado", "PENDIENTE");

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
        public async Task UpdateInitialLoadEstadoAsync(InitialLoadDTO obj)
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
                            string sql = "UPDATE saldos_iniciales SET estado = @estado WHERE folio = @folio;";

                            using (var command = new SqlCommand(sql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@estado", obj.estado);
                                command.Parameters.AddWithValue("@folio", obj.folio);

                                await command.ExecuteNonQueryAsync();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
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

        
        public async Task InsertReporteAsync(ReporteDTO obj)
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
                            string sql = "INSERT INTO reporte (folio, periodo, storage_bin, material_number, material_descripcion, unit_standard_cost, cantidad_inicial, cantidad_contada, diferencia_cantidad, porcentaje_diferencia, importe_inicial, importe_contada, diferencia_importe, porcentaje_variacion_importe, usuario, fecha ) " +
                                            "VALUES (@folio, @periodo, @storage_bin, @material_number, @material_descripcion, @unit_standar_cost, @cantidad_inicial, @cantidad_contada, @diferencia_cantidad, @porcentaje_diferencia, @importe_inicial, @importe_contada, @diferencia_importe, @porcentaje_variacion_importe, @usuario, GETDATE());";

                            using (var command = new SqlCommand(sql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@folio", obj.folio);
                                command.Parameters.AddWithValue("@periodo", obj.periodo);
                                command.Parameters.AddWithValue("@storage_bin", obj.storage_bin);
                                command.Parameters.AddWithValue("@material_number", obj.material_number);
                                command.Parameters.AddWithValue("@material_descripcion", obj.material_descripcion);
                                command.Parameters.AddWithValue("@unit_standar_cost", obj.unit_standard_cost);
                                command.Parameters.AddWithValue("@cantidad_inicial", obj.cantidad_inicial);
                                command.Parameters.AddWithValue("@cantidad_contada", obj.cantidad_contada);
                                command.Parameters.AddWithValue("@diferencia_cantidad", obj.diferencia_cantidad);
                                command.Parameters.AddWithValue("@porcentaje_diferencia", obj.porcentaje_diferencia);
                                command.Parameters.AddWithValue("@importe_inicial", obj.importe_inicial);
                                command.Parameters.AddWithValue("@importe_contada", obj.importe_contada);
                                command.Parameters.AddWithValue("@diferencia_importe", obj.diferencia_importe);
                                command.Parameters.AddWithValue("@porcentaje_variacion_importe", obj.porcentaje_variacion_importe);
                                command.Parameters.AddWithValue("@usuario", obj.usuario);

                                await command.ExecuteNonQueryAsync();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
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
        public async Task<List<ReporteDTO>> GetReportePorPeriodoAsync(string periodo)
        {
            var dtos = new List<ReporteDTO>();
            try
            {
                using (var connection = new SqlConnection(_connectionSQL))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM reporte WHERE periodo = @Periodo;";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Periodo", periodo);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dtos.Add(new ReporteDTO
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    folio = reader["folio"].ToString(),
                                    periodo = reader["periodo"].ToString(),
                                    storage_bin = reader["storage_bin"].ToString(),
                                    material_number = reader["material_number"].ToString(),
                                    material_descripcion = reader["material_descripcion"].ToString(),
                                    unit_standard_cost = double.Parse(reader["unit_standard_cost"].ToString()),
                                    cantidad_inicial = double.Parse(reader["cantidad_inicial"].ToString()),
                                    cantidad_contada = double.Parse(reader["cantidad_contada"].ToString()),
                                    diferencia_cantidad = double.Parse(reader["diferencia_cantidad"].ToString()),
                                    porcentaje_diferencia = double.Parse(reader["porcentaje_diferencia"].ToString()),
                                    importe_inicial = double.Parse(reader["importe_inicial"].ToString()),
                                    importe_contada = double.Parse(reader["importe_contada"].ToString()),
                                    diferencia_importe = double.Parse(reader["diferencia_importe"].ToString()),
                                    porcentaje_variacion_importe = double.Parse(reader["porcentaje_variacion_importe"].ToString()),
                                    usuario = reader["usuario"].ToString(),
                                    fecha = Convert.ToDateTime(reader["periodo"].ToString()),
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
    }
}
