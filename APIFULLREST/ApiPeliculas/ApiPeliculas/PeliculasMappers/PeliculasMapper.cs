using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
        }
    }
}
