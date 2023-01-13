using System.Net;

// Centralizacion de la respuestas de mi web API para tener un objeto común a través del cual yo puedo acceder.
// Por ejemplo, si hubo un error o no en el API, cuál fue la respuesta, etcétera Entonces yo voy a venir




namespace BlazorPeliculas.Client.Repositorios
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> ObtenerMensajeError()
        {
            if (!Error)
            {
                return null;
            }

            var codigoEstatus = HttpResponseMessage.StatusCode;

            if (codigoEstatus == HttpStatusCode.NotFound) 
            {
                return "Recurso no encontrado";
            }
            else if (codigoEstatus == HttpStatusCode.BadRequest)//error en el cuerpo de la respuesta http
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (codigoEstatus == HttpStatusCode.Unauthorized) //el usuario debe estar registrado para realizar una accion
            {
                return "Tienes que loguearte para hacer esto";
            }
            else if (codigoEstatus == HttpStatusCode.Forbidden) //ya estas registrado pero no tiene permiso para realizar determinada accion 
            {
                return "No tienes permisos para hacer esto";
            }
            else
            {
                return "Ha ocurrido un error inesperado"; 
            }
        }
    }
}


//IR A REPOSITORIO 