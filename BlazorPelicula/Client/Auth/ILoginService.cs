using BlazorPelicula.Shared.DTOs;

namespace BlazorPelicula.Client.Auth
{
    public interface ILoginService
    {
        Task Login(string token);
        Task Logout();
        //Task ManejarRenovacionToken();
    }
}
