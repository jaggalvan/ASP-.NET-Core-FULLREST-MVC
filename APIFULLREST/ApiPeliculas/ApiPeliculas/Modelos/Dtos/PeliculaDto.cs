﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dtos
{
    public class PeliculaDto
    {

            public int Id { get; set; }
            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string Nombre { get; set; }
            public string RutaImagen { get; set; }
            [Required(ErrorMessage = "La descripcion es obligatoria")]
            public string Descripcion { get; set; }
            [Required(ErrorMessage = "La duracion es obligatoria")]
            public int duracion { get; set; }
            public enum TipoClasificacion { siete, Trce, Dieciseis, Dieciocho }
            public TipoClasificacion Clasificacion { get; set; }
            public DateTime FechaCreacion { get; set; }
            public int categoriaId { get; set; }
           

    }
}
