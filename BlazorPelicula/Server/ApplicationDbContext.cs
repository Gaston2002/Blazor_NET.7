using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Aqui se representan la clase que van ser entidades(Carpeta Entidades) de mi proyecto.
// Se descargaron dos paquetes Microsoft.EntityFrameworkCore.Sq y Microsoft.EntityFrameworkCore.Tool
//COMANDOS PARA DASE DE DATOS : Add-Migration...nombre   Update-Database

namespace BlazorPeliculas.Server
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) //Representa a que base de datos nos vamos a conectar
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //Llaves compuestas 
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.GeneroId, x.PeliculaId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.ActorId, x.PeliculaId });
        }


        //Creacion de DbSet a partir de Set 
        //Asignado nombres a la tabla 
        public DbSet<Genero> Generos => Set<Genero>(); 
        public DbSet<Actor> Actores => Set<Actor>();
        public DbSet<Pelicula> Peliculas => Set<Pelicula>();
        public DbSet<VotoPelicula> VotoPeliculas => Set<VotoPelicula>();
        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();
        public DbSet<PeliculaActor> PeliculasActores => Set<PeliculaActor>();
    }
}
