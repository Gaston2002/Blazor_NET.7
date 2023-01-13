using Microsoft.EntityFrameworkCore;

namespace BlazorPelicula.Server.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnRespuesta<T>(
            this HttpContext context, IQueryable<T> queryable, int cantidadRegistrosAMostrar
            )
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));

            }

            double conteo = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling( conteo / cantidadRegistrosAMostrar);
            context.Response.Headers.Add("totalPaginas", totalPaginas.ToString() );
        }

    }
}
