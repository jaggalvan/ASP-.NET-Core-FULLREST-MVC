﻿using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

    }
}
