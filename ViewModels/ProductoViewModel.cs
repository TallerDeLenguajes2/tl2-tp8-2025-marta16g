using System;
using System.ComponentModel.DataAnnotations;

namespace EspacioViewModels
{
    public class ProductoViewModel
    {
        [Display(Name = "Descripción")]
        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres")]
        public string Descripcion { get; set; }
        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(1, 99999.99, ErrorMessage = "El precio debe ser un número entre 1 y 99999.99")]
        public double Precio { get; set; }
        public int IdProducto { get; set; }
    }
}