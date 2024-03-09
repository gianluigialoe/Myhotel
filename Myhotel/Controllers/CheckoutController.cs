using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using Myhotel.Models;

namespace Myhotel.Controllers
{
    public class CheckoutController : Controller
    {
        [Authorize]
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OttieniInformazioniConPrezzoTotale()
        {
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();
            decimal prezzoBase = 100;

            // Lista per memorizzare le prenotazioni
            var prenotazioni = new List<PrenotazioneViewModel>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT " +
                                "Clienti.Nome, " +
                                "Clienti.Cognome, " +
                                "Camere.Numero AS NumeroCamera, " +
                                "Prenotazioni.DataInizio, " +
                                "Prenotazioni.DataFine, " +
                                "Prenotazioni.CaparraConfirmatoria, " +
                                "Prenotazioni.TariffaApplicata, " +
                                "Prenotazioni.MezzaPensione, " +
                                "Prenotazioni.PensioneCompleta, " +
                                "Prenotazioni.PernottamentoColazione, " +
                                "ServiziAggiuntivi.Quantita, " +
                                "ServiziAggiuntivi.Prezzo " +
                              "FROM " +
                                "Clienti " +
                                "JOIN Prenotazioni ON Clienti.CodiceFiscale = Prenotazioni.CodiceFiscale " +
                                "JOIN Camere ON Prenotazioni.NumeroCamera = Camere.Numero " +
                                "LEFT JOIN ServiziAggiuntivi ON Prenotazioni.ID = ServiziAggiuntivi.IDPrenotazione";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Ottieni i dati necessari dal lettore SqlDataReader
                            string nome = reader["Nome"].ToString();
                            string cognome = reader["Cognome"].ToString();
                            int numeroCamera = Convert.ToInt32(reader["NumeroCamera"]);
                            DateTime dataInizio = Convert.ToDateTime(reader["DataInizio"]);
                            DateTime dataFine = Convert.ToDateTime(reader["DataFine"]);
                            decimal caparra = Convert.ToDecimal(reader["CaparraConfirmatoria"]);
                            bool mezzaPensione = (bool)reader["MezzaPensione"];
                            bool pensioneCompleta = (bool)reader["PensioneCompleta"];
                            bool pernottamentoColazione = (bool)reader["PernottamentoColazione"];
                            int quantitaServizio = Convert.ToInt32(reader["Quantita"]);
                            decimal prezzoServizioAggiuntivo = Convert.ToDecimal(reader["Prezzo"]);

                            // Esegui i calcoli necessari
                            int giorniPrenotazione = (int)(dataFine - dataInizio).TotalDays;
                            decimal prezzoTotale = prezzoBase * giorniPrenotazione + prezzoServizioAggiuntivo - caparra;

                            // Aggiungi il prezzo per mezza pensione, pensione completa o pernottamento con colazione
                            if (mezzaPensione)
                                prezzoTotale += 50; // Sostituisci con il prezzo specifico
                            else if (pensioneCompleta)
                                prezzoTotale += 80; // Sostituisci con il prezzo specifico
                            else if (pernottamentoColazione)
                                prezzoTotale += 30; // Sostituisci con il prezzo specifico

                            // Creazione di un oggetto PrenotazioneViewModel per rappresentare i dati
                            var prenotazione = new PrenotazioneViewModel(prezzoBase)
                            {
                                Nome = nome,
                                Cognome = cognome,
                                NumeroCamera = numeroCamera,
                                DataInizio = dataInizio,
                                DataFine = dataFine,
                                Caparra = caparra,
                                MezzaPensione = mezzaPensione,
                                PensioneCompleta = pensioneCompleta,
                                PernottamentoColazione = pernottamentoColazione,
                                QuantitaServizio = quantitaServizio,
                                PrezzoServizioAggiuntivo = prezzoServizioAggiuntivo,
                                PrezzoTotale = prezzoTotale
                            };

                            // Aggiungi la prenotazione alla lista
                            prenotazioni.Add(prenotazione);
                        }
                    }
                }
            }

            // Passaggio della lista di prenotazioni alla vista
            return View("Index", prenotazioni);
        }
    }

}