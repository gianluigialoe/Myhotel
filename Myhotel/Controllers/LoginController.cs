using Myhotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;

namespace Myhotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

        // Azione per la visualizzazione della pagina di login
        public ActionResult Index()
        {
            // Verifica se l'utente è già autenticato e, in caso affermativo, reindirizza alla pagina "Prova"
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Dipendenti dipendente)
        {
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Query SQL per cercare l'utente nel database
                    var query = "SELECT * FROM Dipendenti WHERE Username = @Username AND Password = @Password";
                    var command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@Username", dipendente.Username);
                    command.Parameters.AddWithValue("@Password", dipendente.Password);

                    // Esecuzione della query e lettura del risultato
                    var reader = command.ExecuteReader();

                    // Se ci sono righe nel risultato, l'utente è valido
                    if (reader.HasRows)
                    {
                        reader.Read();

                        // Imposta il cookie di autenticazione con l'ID e reindirizza alla pagina "Prova"
                        FormsAuthentication.SetAuthCookie(reader["ID"].ToString(), true);
                        return RedirectToAction("Index", "BackOffice");
                    }

                    // Se l'utente non è valido, aggiungi un errore di modello
                    ModelState.AddModelError(string.Empty, "Credenziali non valide");
                }
            }

            // Se il modello non è valido o ci sono errori, reindirizza alla pagina di login con gli errori
            return View("Index", dipendente);
        }

      

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Cancella l'identità dell'utente
            FormsAuthentication.SignOut();

            // Cancella eventuali altri cookie o informazioni di sessione

            // Ridireziona l'utente alla pagina "Index" del controller "Home"
            return RedirectToAction("Index", "Home");
        }
    }
}