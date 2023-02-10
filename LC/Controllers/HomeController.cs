using LC.Data;
using LC.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace LC.Controllers
{
	public class HomeController : Controller
	{
        LCDbContext db = new LCDbContext();
        public static string? FullLink { get; set; }
        public static string? ShortLink { get; set; }
        public IActionResult Index()
		{            
            var links = db.Links.ToList();
            ViewBag.Links = links;
            ViewBag.ShortLink = ShortLink;
            ViewBag.FullLink = FullLink;
            return View();
		}

        private string RandomId()
        {
            const string hashSymbols = "0123456789abcdefghijklmnopqrstuvwxyzABSDEFGHIJKLMNOPQRSTUVWXYZ";
            Random randomiser = new Random();
            string hashCode = string.Empty;
            const byte hashLength = 9;
            for (int i = 0; i < hashLength; i++)
            {
                hashCode += hashSymbols[randomiser.Next(0, hashSymbols.Length)];
            }
            return hashCode;
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (id == null || id.Contains(" ") || id.Length != 9)
            {
                return RedirectToAction("Delete");
            }
            LinkModel? link = db.Links.Find(id);
            if (link != null)
            {
                db.Links.Remove(link);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Create(string fullLink)
        {
            if (fullLink == null || fullLink.Contains(" ")) 
            { 
                return RedirectToAction("Create"); 
            }

            var links = db.Links.ToList();
            string id = RandomId();
            const string shortLink = "http://lcs.biz/";

            while (links.Any(link => link.Id == id))
            {
                id = RandomId();
            }

            FullLink = fullLink;
            ShortLink = shortLink + id;

            db.Add(new LinkModel
            {
                Id = id,
                Full = fullLink,
                DateOfCreating = DateTime.Now,
                NumberOfTransitions = 0
            });

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}
