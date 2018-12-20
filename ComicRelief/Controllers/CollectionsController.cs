using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComicRelief.Models;

namespace ComicRelief.Controllers
{
    public class CollectionsController : Controller
    {
        private ComicsEntities db = new ComicsEntities();
        private string _apiKeyPublic = "ee2d3c4214a5a8c4459575b1387bcd2a";
        private string _apiKeyPrivte = "5732c3459019c85194f95c736e01c494d700865d";

        // GET: Collections
        public ActionResult Index()
        {
            return View(db.Collections.ToList());
        }

        // GET: Collections/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // GET: Collections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Collections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,issueNumber,isbn,Description,ComicID,Publisher")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                db.Collections.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(collection);
        }

        // GET: Collections/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,issueNumber,isbn,Description,ComicID,Publisher")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(collection);
        }

        // GET: Collections/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Collection collection = db.Collections.Find(id);
            db.Collections.Remove(collection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCollection(int comicId)
        {
            Marvel marvelClient = new Marvel(_apiKeyPublic, _apiKeyPrivte);
            Comic comic = marvelClient.GetComic(comicId);
            Collection c = new Collection();

            c.ComicID = comic.Id.ToString();
            c.Description = comic.Description;
            c.isbn = comic.ISBN;
            c.Publisher = "Marvel";
            c.Title = comic.Title;
            c.issueNumber = comic.IssueNumber.ToString();

            if (ModelState.IsValid)
            {
                db.Collections.Add(c);
                db.SaveChanges();
                
            }
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
