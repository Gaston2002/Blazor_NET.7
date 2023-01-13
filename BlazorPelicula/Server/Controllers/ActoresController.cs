using BlazorPelicula.Server.Helpers;
using BlazorPeliculas.Server;
using BlazorPeliculas.Shared.Entidades;
using BlazorPeliculas.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using AutoMapper;
using System.Reflection.Metadata.Ecma335;
using BlazorPelicula.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

//La clases controller se encargan de recibir peticiones HTTP esto permite que interactuemos con la base de Datos. 
// usamos el prefijo "api" para llamar a los controladores que tiene que ver con la WebApi 




namespace BlazorPelicula.Server.Controllers
{
    [ApiController]
    [Route("api/actores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "personas";

        public ActoresController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper) //clase program
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacion.CantidadRegistros);

            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacion).ToListAsync();
        }




        [HttpGet("id:int")]
        public async Task<ActionResult<Actor>> Get(int id)
        {
            Console.WriteLine("Get(int id)");
            var actor = await context.Actores.FirstOrDefaultAsync(actor => actor.Id == id);

            if (actor is null)
            {
                return NotFound();
            }

            return actor;

        }




        [HttpGet("buscar/{textoBusqueda}")]
        public async Task<ActionResult<List<Actor>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Actor>();
            }

            textoBusqueda = textoBusqueda.ToLower();

            return await context.Actores
                        .Where(actor => actor.Nombre.ToLower().Contains(textoBusqueda))
                        .Take(5)
                        .ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post(Actor actor)
        {
            if (!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actor.Foto = await almacenadorArchivos.GuardarArchivo(fotoActor, "jpg", contenedor);
            }


            //throw new NotSupportedException();
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;


        }



        [HttpPut] //CREACION DE ACTOR EN BASE DE DATOS 
        public async Task<ActionResult> Put(Actor actor)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(a => a.Id == actor.Id);


            if (actorDB is null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(actor, actorDB);
            

            if (!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor. Foto);
                actorDB.Foto = await almacenadorArchivos.EditarArchivo(fotoActor, ".jpg",
                    contenedor, actorDB.Foto!);
            }


            await context.SaveChangesAsync();
            return NoContent();


        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
          var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor is null)
            {
                return NotFound();
            }
            
            context.Remove(actor);
            context.SaveChanges();

            await almacenadorArchivos.EliminarArchivo(actor.Foto!, contenedor);

            return NoContent();
        }

    }
}
