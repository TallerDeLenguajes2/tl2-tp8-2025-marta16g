using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EspacioViewModels
{
    public class PresupustoViewModel
    {
        [Display(Name = "Nombre del destinatario")]
        [Required(ErrorMessage = "El destinatario es obligatorio")]
        public string NombreDestinatario { get; set; }
        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }
    }
}