using AppLogin.DTOs.Excel;

namespace AppLogin.Repos
{
    public interface IBitacora
    {
        Task InsertBitacoraAsync(BitacoraDTO model);
    }
}
