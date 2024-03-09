using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Myhotel.Models
{
    public class Dipendenti
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Min 3, max 20 caratteri")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Min 2, max 15 caratteri")]
        public string Password { get; set; }
    }
}