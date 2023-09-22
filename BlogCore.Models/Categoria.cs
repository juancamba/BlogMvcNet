using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la categoria no puede ser vacio")]
        [Display(Name = "Nombre Categoria")]
        public string Nombre { get; set; }

        [Display(Name = "Orden visualización")]
        public int? Orden { get; set; }

    }
}
