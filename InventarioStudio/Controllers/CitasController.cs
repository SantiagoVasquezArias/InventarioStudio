using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventarioStudioDAO;
using System.Text;
using System.Net.Mail;

namespace InventarioStudio.Controllers
{
    public class CitasController : Controller
    {
        public static void EnviarCorreo(string destino, string asunto, string cuerpo)
        {
            try
            {

                //Configuración del Mensaje

                MailMessage mail = new MailMessage();


                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //correo desde el que se enviará el Email y  nombre de la persona que lo envía
                mail.From = new MailAddress("santiagovasquezarias@gmail.com", "Sistema de Inventario de music", Encoding.UTF8);
                // asunto del correo


                mail.To.Add(destino);
                mail.Subject = asunto.Replace("\r\n", " ");
                mail.Body = $"{cuerpo}";
                mail.IsBodyHtml = true;
                //Configuracion del SMTP                                    
                SmtpServer.Port = 587; //Puerto que utiliza Gmail para sus servicios
                //credenciales con las que enviaremos el mail
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Timeout = 10000;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                SmtpServer.Credentials = new System.Net.NetworkCredential("santiagovasquezarias@gmail.com", "Motleycrue1991..");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }

        private DbInventariosStudioEntities db = new DbInventariosStudioEntities();

        // GET: Citas
        public ActionResult Index()
        {
            var cita = db.Cita.Include(c => c.Sala);
            return View(cita.ToList());
        }

        // GET: Citas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cita cita = db.Cita.Find(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            return View(cita);
        }

        // GET: Citas/Create
        public ActionResult Create()
        {
            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre");
            return View();
        }

        // POST: Citas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdSala,Nombre,Apellido,Cel,FechaHoraInico,FechaHoraFin,Correo")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                db.Cita.Add(cita);
                db.SaveChanges();

                var sala = db.Sala.FirstOrDefault(x => x.Id == cita.IdSala);

                EnviarCorreo(cita.Correo, "Recodatorio de sesion de estudio", "Usted tiene una sesion de estudio en la   "  + sala.Nombre +  ", Hora inicio " + cita.FechaHoraInico +
              " y Fecha finalización " + cita.FechaHoraFin + " con el productor " + cita.Sala.Usuario.NombreArtistico + ".");
                return RedirectToAction("Index");
            }

            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre", cita.IdSala);
            return View(cita);
        }

        // GET: Citas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cita cita = db.Cita.Find(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre", cita.IdSala);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdSala,Nombre,Apellido,Cel,FechaHoraInico,FechaHoraFin,Correo")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cita).State = EntityState.Modified;
                db.SaveChanges();

                var sala = db.Sala.FirstOrDefault(x => x.Id == cita.IdSala);

                EnviarCorreo(cita.Correo, "Recodatorio de sesion de estudio", "Se ha modificado la sesion de estudio en la   " + sala.Nombre + " ,Hora inicio " + cita.FechaHoraInico +
             " y Fecha finalización " + cita.FechaHoraFin + " con el productor " + cita.Sala.Usuario.NombreArtistico + ".");
                return RedirectToAction("Index");               
            }
            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre", cita.IdSala);          
            return View(cita);
        }

        // GET: Citas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cita cita = db.Cita.Find(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cita cita = db.Cita.Find(id);
            db.Cita.Remove(cita);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
