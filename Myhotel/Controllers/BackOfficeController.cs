using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Configuration;
using System.Data.SqlClient;
using Myhotel.Models;
using System.Transactions;

namespace Myhotel.Controllers
{
    [Authorize]
    public class BackOfficeController : Controller
    {

        public ActionResult Index()
        {

            try
            {
                var DipendenteId = HttpContext.User.Identity.Name;
                ViewBag.DipendenteId = DipendenteId;

                string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();
                List<Prenotazione> prenotazioniList = new List<Prenotazione>();

                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();

                    var selectCommand = new SqlCommand("SELECT * FROM Prenotazioni", conn);

                    using (var reader = selectCommand.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Prenotazione prenotazione = new Prenotazione
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                CodiceFiscaleCliente = Convert.ToString(reader["CodiceFiscale"]),
                                NumeroCamera = Convert.ToInt32(reader["NumeroCamera"]),
                                DataPrenotazione = Convert.ToDateTime(reader["DataPrenotazione"]),
                                NumeroProgressivoAnno = Convert.ToInt32(reader["NumeroProgressivoAnno"]),
                                Anno = Convert.ToInt32(reader["Anno"]),
                                DataInizio = Convert.ToDateTime(reader["DataInizio"]),
                                DataFine = Convert.ToDateTime(reader["DataFine"]),
                                CaparraConfirmatoria = Convert.ToDecimal(reader["CaparraConfirmatoria"]),
                                TariffaApplicata = Convert.ToDecimal(reader["TariffaApplicata"]),
                                MezzaPensione = Convert.ToBoolean(reader["MezzaPensione"]),
                                PensioneCompleta = Convert.ToBoolean(reader["PensioneCompleta"]),
                                PernottamentoColazione = Convert.ToBoolean(reader["PernottamentoColazione"])
                            };

                            prenotazioniList.Add(prenotazione);
                        }

                    }
                }

                // Passa la lista di prenotazioni alla vista solo se ci sono dati
                if (prenotazioniList.Count > 0)
                {
                    return View(prenotazioniList);
                }
                else
                {
                    ViewBag.ErrorMessage = "Nessuna prenotazione trovata.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Si è verificato un errore: {ex.Message}";
                return View("Error");
            }
        }


        public ActionResult AddCliente()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCliente([Bind(Include = "CodiceFiscale,Cognome,Nome,Citta,Provincia,Email,Telefono,Cellulare,")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

                    var conn = new SqlConnection(connString);
                    {
                        conn.Open();

                        var command = new SqlCommand(@"
                INSERT INTO Clienti
                (CodiceFiscale, Cognome, Nome, Citta, Provincia, Email, Telefono, Cellulare)
                VALUES (@codiceFiscale, @cognome, @nome, @citta, @provincia, @email, @telefono, @cellulare)", conn);

                        command.Parameters.AddWithValue("@codiceFiscale", cliente.CodiceFiscale);
                        command.Parameters.AddWithValue("@cognome", cliente.Cognome);
                        command.Parameters.AddWithValue("@nome", cliente.Nome);
                        command.Parameters.AddWithValue("@citta", cliente.Citta);
                        command.Parameters.AddWithValue("@provincia", cliente.Provincia);
                        command.Parameters.AddWithValue("@email", cliente.Email);

                        // Assicurati che i valori di Telefono e Cellulare siano gestiti correttamente

                        command.Parameters.AddWithValue("@telefono", cliente.Telefono);


                        command.Parameters.AddWithValue("@cellulare", cliente.Cellulare);




                        var numRows = command.ExecuteNonQuery();
                    }

                    return RedirectToAction("AddCamera", "BackOffice");
                    // Ridireziona alla pagina desiderata
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                    // Aggiungi altri dettagli dell'errore, come ex.StackTrace, se necessario

                    ViewBag.ErrorMessage = $"Si è verificato un errore: {ex.Message}";
                    return View("Error");
                }
            }

            ViewBag.isValid = false;
            return View(cliente);
        }




        public ActionResult AddCamera()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCamera([Bind(Include = "Numero,Descrizione,Tipologia")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        var commandCamere = new SqlCommand(@"
                    INSERT INTO Camere
                    (Numero, Descrizione, Tipologia)
                    VALUES (@numero, @descrizione, @tipologia)", conn);

                        // Impostiamo i parametri per la tabella Camere
                        commandCamere.Parameters.AddWithValue("@numero", camera.Numero);
                        commandCamere.Parameters.AddWithValue("@descrizione", camera.Descrizione);
                        commandCamere.Parameters.AddWithValue("@tipologia", camera.Tipologia);

                        var numRowsCamere = commandCamere.ExecuteNonQuery();
                    }

                    return RedirectToAction("AddPrenotazione", "BackOffice"); // Ridireziona alla pagina desiderata
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Si è verificato un errore: {ex.Message}";
                    return View("Error");
                }
            }

            ViewBag.isValid = false;
            return View(camera);
        }

        public ActionResult AddPrenotazione()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrenotazione([Bind(Include = "CodiceFiscaleCliente,NumeroCamera,DataPrenotazione,NumeroProgressivoAnno,Anno,DataInizio,DataFine,CaparraConfirmatoria,TariffaApplicata,MezzaPensione,PensioneCompleta,PernottamentoColazione")] Prenotazione prenotazione)
        {
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                // Inizia una transazione
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        // Ottieni la stringa di connessione dal file di configurazione
                        string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

                        // Apre una connessione al database
                        using (var conn = new SqlConnection(connString))
                        {
                            conn.Open();

                            // Verifica che il Codice Fiscale esista nella tabella Clienti
                            var verificaClienteCommand = new SqlCommand("SELECT COUNT(*) FROM Clienti WHERE CodiceFiscale = @codiceFiscale", conn);
                            verificaClienteCommand.Parameters.AddWithValue("@codiceFiscale", prenotazione.CodiceFiscaleCliente);

                            int countCliente = (int)verificaClienteCommand.ExecuteScalar();

                            // Se il Codice Fiscale non esiste nella tabella Clienti, mostra un messaggio di errore
                            if (countCliente == 0)
                            {
                                ViewBag.ErrorMessage = "Il codice fiscale specificato non esiste nella tabella Clienti.";
                                return View("Error");
                            }

                            // Verifica che il Numero Camera esista nella tabella Camere
                            var verificaCameraCommand = new SqlCommand("SELECT COUNT(*) FROM Camere WHERE Numero = @numeroCamera", conn);
                            verificaCameraCommand.Parameters.AddWithValue("@numeroCamera", prenotazione.NumeroCamera);

                            int countCamera = (int)verificaCameraCommand.ExecuteScalar();

                            // Se il Numero Camera non esiste nella tabella Camere, mostra un messaggio di errore
                            if (countCamera == 0)
                            {
                                ViewBag.ErrorMessage = "Il numero della camera specificato non esiste nella tabella Camere.";
                                return View("Error");
                            }

                            // Continua con l'inserimento nella tabella Prenotazioni
                            var commandPrenotazioni = new SqlCommand(@"
                        INSERT INTO Prenotazioni
                        (CodiceFiscale, NumeroCamera, DataPrenotazione, NumeroProgressivoAnno, Anno, DataInizio, DataFine, CaparraConfirmatoria, TariffaApplicata, MezzaPensione, PensioneCompleta, PernottamentoColazione)
                        VALUES (@codiceFiscale, @numeroCamera, @dataPrenotazione, @numeroProgressivoAnno, @anno, @dataInizio, @dataFine, @caparraConfirmatoria, @tariffaApplicata, @mezzaPensione, @pensioneCompleta, @pernottamentoColazione)", conn);

                            // Impostiamo i parametri per la tabella Prenotazioni
                            commandPrenotazioni.Parameters.AddWithValue("@codiceFiscale", prenotazione.CodiceFiscaleCliente);
                            commandPrenotazioni.Parameters.AddWithValue("@numeroCamera", prenotazione.NumeroCamera);
                            commandPrenotazioni.Parameters.AddWithValue("@dataPrenotazione", prenotazione.DataPrenotazione);
                            commandPrenotazioni.Parameters.AddWithValue("@numeroProgressivoAnno", prenotazione.NumeroProgressivoAnno);
                            commandPrenotazioni.Parameters.AddWithValue("@anno", prenotazione.Anno);
                            commandPrenotazioni.Parameters.AddWithValue("@dataInizio", prenotazione.DataInizio);
                            commandPrenotazioni.Parameters.AddWithValue("@dataFine", prenotazione.DataFine);
                            commandPrenotazioni.Parameters.AddWithValue("@caparraConfirmatoria", prenotazione.CaparraConfirmatoria);
                            commandPrenotazioni.Parameters.AddWithValue("@tariffaApplicata", prenotazione.TariffaApplicata);
                            commandPrenotazioni.Parameters.AddWithValue("@mezzaPensione", prenotazione.MezzaPensione);
                            commandPrenotazioni.Parameters.AddWithValue("@pensioneCompleta", prenotazione.PensioneCompleta);
                            commandPrenotazioni.Parameters.AddWithValue("@pernottamentoColazione", prenotazione.PernottamentoColazione);

                            // Esegui l'inserimento e ottieni il numero di righe interessate
                            var numRowsPrenotazioni = commandPrenotazioni.ExecuteNonQuery();
                        }

                        // Completa la transazione
                        scope.Complete();

                        // Ridireziona alla pagina desiderata
                        return RedirectToAction("AddServizioAggiuntivo", "BackOffice");
                    }
                    catch (Exception ex)
                    {
                        // In caso di errore, mostra un messaggio di errore
                        ViewBag.ErrorMessage = $"Si è verificato un errore: {ex.Message}";
                        return View("Error");
                    }
                }
            }

            // Se il modello non è valido, restituisci la vista con gli errori di validazione
            return View(prenotazione);
        }



        public ActionResult AddServizioAggiuntivo()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddServizioAggiuntivo([Bind(Include = "IDPrenotazione,DataServizio,Quantita,Prezzo,Descrizione")] ServizioAggiuntivo servizioAggiuntivo)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

                        using (var conn = new SqlConnection(connString))
                        {
                            conn.Open();

                            // Verifica che l'IDPrenotazione esista nella tabella Prenotazioni
                            var verificaPrenotazioneCommand = new SqlCommand("SELECT COUNT(*) FROM Prenotazioni WHERE ID = @idPrenotazione", conn);
                            verificaPrenotazioneCommand.Parameters.AddWithValue("@idPrenotazione", servizioAggiuntivo.IDPrenotazione);

                            int countPrenotazione = (int)verificaPrenotazioneCommand.ExecuteScalar();

                            if (countPrenotazione == 0)
                            {
                                // IDPrenotazione non trovato nella tabella Prenotazioni
                                ViewBag.ErrorMessage = "L'IDPrenotazione specificato non esiste nella tabella Prenotazioni.";
                                return View("Error");
                            }

                            // Continua con l'inserimento nella tabella ServiziAggiuntivi
                            var commandServiziAggiuntivi = new SqlCommand(@"
                        INSERT INTO ServiziAggiuntivi
                        (IDPrenotazione, DataServizio, Quantita, Prezzo, Descrizione)
                        VALUES (@idPrenotazione, @dataServizio, @quantita, @prezzo, @descrizione)", conn);

                            // Impostiamo i parametri per la tabella ServiziAggiuntivi
                            commandServiziAggiuntivi.Parameters.AddWithValue("@idPrenotazione", servizioAggiuntivo.IDPrenotazione);
                            commandServiziAggiuntivi.Parameters.AddWithValue("@dataServizio", servizioAggiuntivo.DataServizio);
                            commandServiziAggiuntivi.Parameters.AddWithValue("@quantita", servizioAggiuntivo.Quantita);
                            commandServiziAggiuntivi.Parameters.AddWithValue("@prezzo", servizioAggiuntivo.Prezzo);
                            commandServiziAggiuntivi.Parameters.AddWithValue("@descrizione", servizioAggiuntivo.Descrizione);

                            var numRowsServiziAggiuntivi = commandServiziAggiuntivi.ExecuteNonQuery();
                        }

                        scope.Complete(); // Completa la transazione
                        return RedirectToAction("Index"); // Ridireziona alla pagina desiderata
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = $"Si è verificato un errore: {ex.Message}";
                        return View("Error");
                    }
                }
            }

            ViewBag.isValid = false;
            return View(servizioAggiuntivo);
        }


    }
}
