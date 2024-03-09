using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myhotel.Models
{
    public class PrenotazioneViewModel
    {
        // Proprietà della prenotazione
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public int NumeroCamera { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public decimal Caparra { get; set; }
        public bool MezzaPensione { get; set; }
        public bool PensioneCompleta { get; set; }
        public bool PernottamentoColazione { get; set; }
        public decimal PrezzoServizioAggiuntivo { get; set; }
        public decimal PrezzoTotale { get; set; }
        public int QuantitaServizio { get; set; } // Aggiunta della nuova proprietà

        // Costruttore che accetta i valori di base
        public PrenotazioneViewModel(decimal prezzoBase)
        {
            PrezzoBase = prezzoBase;
        }

        // Proprietà aggiunta per il prezzo di base
        public decimal PrezzoBase { get; }
    }
}