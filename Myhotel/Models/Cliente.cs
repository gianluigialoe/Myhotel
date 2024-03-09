using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Myhotel.Models
{
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "Il campo Codice Fiscale è obbligatorio.")]
        public string CodiceFiscale { get; set; }

        [Required(ErrorMessage = "Il campo Cognome è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il campo Cognome può contenere al massimo 50 caratteri.")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il campo Nome può contenere al massimo 50 caratteri.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Città è obbligatorio.")]
        [MaxLength(100, ErrorMessage = "Il campo Città può contenere al massimo 100 caratteri.")]
        public string Citta { get; set; }

        [Required(ErrorMessage = "Il campo Provincia è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il campo Provincia può contenere al massimo 50 caratteri.")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "Il campo Email è obbligatorio.")]
        [MaxLength(100, ErrorMessage = "Il campo Email può contenere al massimo 100 caratteri.")]
        [EmailAddress(ErrorMessage = "Inserire un indirizzo email valido.")]
        public string Email { get; set; }

        [Display(Name = "Telefono")]
        public int? Telefono { get; set; }

        [Display(Name = "Cellulare")]
        public int? Cellulare { get; set; }

     
    }

}