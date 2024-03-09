using Myhotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace Myhotel.Controllers
{
    public class jsonController : Controller
    {
        // GET: json
        public JsonResult GetPrenotazioniByCodiceFiscale(string codiceFiscale)
        {
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();
            List<Prenotazione> prenotazioni = new List<Prenotazione>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                // Esempio di query per ottenere le prenotazioni in base al codice fiscale
                string query = "SELECT * FROM [MyHotel].[dbo].[Prenotazioni] WHERE CodiceFiscale = @CodiceFiscale";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Prenotazione prenotazione = new Prenotazione
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                // Altre proprietà della prenotazione...
                            };
                            prenotazioni.Add(prenotazione);
                        }
                    }
                }
            }

            return Json(prenotazioni, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNumeroPrenotazioniPensioneCompleta()
        {
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();
            int numeroPrenotazioni = 0;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                // Esempio di query per ottenere il numero di prenotazioni per soggiorni di tipo "pensione completa"
                string query = "SELECT COUNT(*) AS NumeroPrenotazioni FROM [MyHotel].[dbo].[Prenotazioni] WHERE PensioneCompleta = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            numeroPrenotazioni = Convert.ToInt32(reader["NumeroPrenotazioni"]);
                        }
                    }
                }
            }

            return Json(new { NumeroPrenotazioni = numeroPrenotazioni }, JsonRequestBehavior.AllowGet);
        }
    }

}