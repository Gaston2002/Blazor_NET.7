using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//La clases controller se encargan de recibir peticiones HTTP esto permite que interactuemos con la base de Datos. 
// usamos el prefijo "api" para llamar a los controladores que tiene que ver con la WebApi 

namespace BlazorPeliculas.Server.Controllers
{
    [Route("api/generos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost] //Cargamos un genero 
        public async Task<ActionResult<int>> Post(Genero genero)
        {
            //throw new NotSupportedException();
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id)
        {

            var genero = await context.Generos.FirstOrDefaultAsync(genero => genero.Id == id);

            if (genero is null)
            {
                return NotFound();


            }

            return genero;
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<ActionResult<IEnumerable<Genero>>> Get()
        {
            return await context.Generos.ToListAsync();
        }

        [HttpPut]
        public async Task<ActionResult> Put(Genero genero)
        {
            context.Update(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filasAfectadas = await context.Generos.Where(x => x.Id == id).ExecuteDeleteAsync(); //borrando

            if (filasAfectadas  == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

    }

}
