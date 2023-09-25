using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{

    public class Articulo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre requerido")]
        [Display(Name = "Nombre del articulo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Descripcion requerido")]
        [Display(Name = "Descripcion del articulo")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Fecha requerido")]
        [Display(Name = "Fecha del articulo")]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen del articulo")]
        public string UrlImagen { get; set; }



        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
