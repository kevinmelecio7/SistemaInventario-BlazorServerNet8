using AppLogin.DTOs;

namespace AppLogin.Repos
{
    public interface IBitacora
    {
        Task InsertBitacoraAsync(BitacoraDTO model);
    }
}
