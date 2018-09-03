using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventarioStudioDAO;

namespace InventarioStudio.Controllers
{
    public class InventariosController : Controller
    {
        private DbInventariosStudioEntities db = new DbInventariosStudioEntities();

        // GET: Inventarios
        public ActionResult Index(String busqueda)
        {
           /* var inventario = db.Inventario.Include(i => i.Sala).Include(i => i.Tipo)*/;

            if (busqueda != null)
            {
                return View(db.Inventario.Where(s => s.Tipo.Nombre.Contains(busqueda) || s.Marca.Contains(busqueda)
               || s.Ram.Contains(busqueda) || s.Procesador.Contains(busqueda) || s.Serial.Contains(busqueda)).ToList());

            }
            else
            {
                return View(db.Inventario.ToList());

                //    return View(inventario.ToList());
            }


          
        }

        // GET: Inventarios/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Tipos = db.Tipo.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventario inventario = db.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }
            return View(inventario);
        }

        // GET: Inventarios/Create
        public ActionResult Create()
        {
            ViewBag.Tipos = db.Tipo.ToList();
            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre");
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nombre");
            return View();
        }

        // POST: Inventarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdTipo,IdSala,Marca,Procesador,DiscoDuro,Ram,Serial,Observacion")] Inventario inventario)
        {
            ViewBag.Tipos = db.Tipo.ToList();

            if (ModelState.IsValid)
            {
                db.Inventario.Add(inventario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tipos = db.Tipo.ToList();
            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre", inventario.IdSala);
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nombre", inventario.IdTipo);
            return View(inventario);
        }

        // GET: Inventarios/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Tipos = db.Tipo.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventario inventario = db.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre", inventario.IdSala);
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nombre", inventario.IdTipo);
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdTipo,IdSala,Marca,Procesador,DiscoDuro,Ram,Serial,Observacion")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSala = new SelectList(db.Sala, "Id", "Nombre", inventario.IdSala);
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nombre", inventario.IdTipo);
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventario inventario = db.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }
            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventario inventario = db.Inventario.Find(id);
            db.Inventario.Remove(inventario);
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
