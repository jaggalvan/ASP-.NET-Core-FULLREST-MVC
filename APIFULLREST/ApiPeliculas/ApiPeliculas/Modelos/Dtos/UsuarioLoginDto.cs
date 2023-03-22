using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Modelos.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El password es obligatorio")]
        public string Password { get; set; }
    }
}
