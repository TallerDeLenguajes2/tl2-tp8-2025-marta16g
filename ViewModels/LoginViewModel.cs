using System;
using System.ComponentModel.DataAnnotations;

namespace EspacioViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Pass { get; set; }
        public string? ErrorMessage { get; set; }
        public bool? IsAuthenticated { get; set; }
    
    }
}