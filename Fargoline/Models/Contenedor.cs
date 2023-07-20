using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fargoline.Models
{
    public class Contenedor
    {
        [Display(Name= "Número del contenedor")]
        public int NumeroContenedor { get; set; }

        [Display(Name = "Tipo del contenedor")]
        [Required(ErrorMessage = "Debe ingresar el tipo del contenedor")]
        public string TipoContenedor { get; set; }

        [Display(Name = "Tamaño del contenedor")]
        [Required(ErrorMessage = "Debe ingresar el tamaño del contenedor")]
        public int TamañoContenedor { get; set; }

        [Display(Name = "Peso del contenedor")]
        [Required(ErrorMessage = "Debe ingresar el peso del contenedor")]
        public decimal PesoContenedor {get; set;}

        [Display(Name = "Tara del contenedor")]
        [Required(ErrorMessage = "Debe ingresar el tara del contenedor")]
        public decimal TaraContenedor { get; set; }

    }
}