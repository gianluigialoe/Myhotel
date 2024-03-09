using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Myhotel.Models
{
    public class ServizioAggiuntivo
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Il campo ID Prenotazione è obbligatorio.")]
        [Display(Name = "ID Prenotazione")]
        public int IDPrenotazione { get; set; }

        [Required(ErrorMessage = "Il campo Data Servizio è obbligatorio.")]
        [Display(Name = "Data Servizio")]
        public DateTime DataServizio { get; set; }

        [Required(ErrorMessage = "Il campo Quantità è obbligatorio.")]
        [Display(Name = "Quantità")]
        public int Quantita { get; set; }

        [Required(ErrorMessage = "Il campo Prezzo è obbligatorio.")]
        [Display(Name = "Prezzo")]
        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio.")]
        [Display(Name = "Descrizione")]
        public string Descrizione { get; set; }
    }
}