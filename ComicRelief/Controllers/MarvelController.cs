using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using ComicRelief.Models;

namespace ComicRelief.Controllers
{
    public class MarvelController : Controller
    {
        private ComicsEntities db = new ComicsEntities();
        private string _apiKeyPublic = "ee2d3c4214a5a8c4459575b1387bcd2a";
        private string _apiKeyPrivte = "5732c3459019c85194f95c736e01c494d700865d";

        public ActionResult Index()
        {
            string id = Request["id"];
            string isbn = Request["isbn"];
            string startsWithTitle = Request["titleStartsWith"];

            Marvel marvelClient = new Marvel(_apiKeyPublic, _apiKeyPrivte);
            IEnumerable<Comic> comics = marvelClient.GetMyComics(isbn, startsWithTitle, id, 50);

            // check to see if comics already exists in collection
            foreach( Comic comic in comics )
            {
                Collection c = db.Collections.Find(comic.Id.ToString());
                if( c == null )
                {
                    comic.IsComicInCollection = false;
                }// end if
                else
                {
                    comic.IsComicInCollection = true;
                }
                
            }// end foreach

            ViewBag.label = "Marvel Comics";
            return View(comics);
        }
    }
}