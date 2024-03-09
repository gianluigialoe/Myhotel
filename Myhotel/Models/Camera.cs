using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Myhotel.Models
{
    public class Camera
    {
        [Key]
        public int Numero { get; set; }

        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio.")]
        [Display(Name = "Descrizione")]
        public string Descrizione { get; set; }

        [Required(ErrorMessage = "Il campo Tipologia è obbligatorio.")]
        [Display(Name = "Tipologia")]
        public string Tipologia { get; set; }
    }
}
