using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorPelicula.Client.Auth
{
    //¿Quien eres?
    public class ProveedorAutenticacionPrueba : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            
            var anonimo = new ClaimsIdentity();
            var usuarioFelipe = new ClaimsIdentity(
                new List<Claim> { new Claim("llave1", "valor1"), new Claim ("edad", "999"), new Claim(ClaimTypes.Name, "Felipe")},
                authenticationType: "prueba");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));
        }
    }
}
