using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Myhotel.Models
{
    public class Prenotazione
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Il campo Codice Fiscale Cliente è obbligatorio.")]
        [Display(Name = "Codice Fiscale Cliente")]
        public string CodiceFiscaleCliente { get; set; }

        [Required(ErrorMessage = "Il campo Numero Camera è obbligatorio.")]
        [Display(Name = "Numero Camera")]
        public int NumeroCamera { get; set; }

        [Required(ErrorMessage = "Il campo Data Prenotazione è obbligatorio.")]
        [Display(Name = "Data Prenotazione")]
        public DateTime DataPrenotazione { get; set; }

        [Required(ErrorMessage = "Il campo Numero Progressivo Anno è obbligatorio.")]
        [Display(Name = "Numero Progressivo Anno")]
        public int NumeroProgressivoAnno { get; set; }

        [Required(ErrorMessage = "Il campo Anno è obbligatorio.")]
        [Display(Name = "Anno")]
        public int Anno { get; set; }

        [Required(ErrorMessage = "Il campo Data Inizio è obbligatorio.")]
        [Display(Name = "Data Inizio")]
        public DateTime DataInizio { get; set; }

        [Required(ErrorMessage = "Il campo Data Fine è obbligatorio.")]
        [Display(Name = "Data Fine")]
        public DateTime DataFine { get; set; }

        [Required(ErrorMessage = "Il campo Caparra Confirmatoria è obbligatorio.")]
        [Display(Name = "Caparra Confirmatoria")]
        public decimal CaparraConfirmatoria { get; set; }

        [Required(ErrorMessage = "Il campo Tariffa Applicata è obbligatorio.")]
        [Display(Name = "Tariffa Applicata")]
        public decimal TariffaApplicata { get; set; }

        [Required(ErrorMessage = "Il campo Mezza Pensione è obbligatorio.")]
        [Display(Name = "Mezza Pensione")]
        public bool MezzaPensione { get; set; }

        [Required(ErrorMessage = "Il campo Pensione Completa è obbligatorio.")]
        [Display(Name = "Pensione Completa")]
        public bool PensioneCompleta { get; set; }

        [Required(ErrorMessage = "Il campo Pernottamento Colazione è obbligatorio.")]
        [Display(Name = "Pernottamento Colazione")]
        public bool PernottamentoColazione { get; set; }
        public object CodiceFiscale { get; internal set; }
    }
}