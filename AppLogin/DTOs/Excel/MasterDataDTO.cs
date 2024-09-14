namespace AppLogin.DTOs.Excel
{
    public class MasterDataDTO
    {
        public int id_material { get; set; }
        public string? materialID { get; set; }
        public string? descripcion { get; set; }
        public double unit_price { get; set; }
        public int fkPeriodo { get; set; }
    }
}
