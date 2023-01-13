using AutoMapper;
using BlazorPelicula.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;

namespace BlazorPelicula.Server.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Actor, Actor>()
                .ForMember(x => x.Foto, option => option.Ignore());

            CreateMap<Pelicula, Pelicula>()
                .ForMember(x => x.Poster, option => option.Ignore());

            CreateMap<VotoPeliculaDTO, VotoPelicula>();
        }

    }
}
