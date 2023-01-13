using BlazorPeliculas.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorPelicula.Client.Auth
{
    public class ProveedorAuteticacionJWT : AuthenticationStateProvider, ILoginService
    {

        //Intancia para poder interactuar con Locar Storage

        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;

        public ProveedorAuteticacionJWT(IJSRuntime js, HttpClient httpClient)
        {
            this.js = js;
            this.httpClient = httpClient;
        }

        public static readonly string TOKENKEY = "TOKENKEY";

        private AuthenticationState Anonimo =>
            (new AuthenticationState(new ClaimsPrincipal()));

      

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.ObtenerDeLocalStorage(TOKENKEY);

            if (token is null)
            {
                return Anonimo;
            }

            return ConstruirAuthenticationState(token.ToString()!);

        }

        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);

            var claims = ParsearClaimsDelJWT(token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        private IEnumerable<Claim> ParsearClaimsDelJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);
            return tokenDeserializado.Claims;
        }

        public async Task Login(string token)
        {
            await js.GuardarEnLocalStorage(TOKENKEY, token);
            var authState = ConstruirAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));


        }

        public async Task Logout()
        {
            await js.RemoverDelLocalStorage(TOKENKEY);
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }
    }
}
