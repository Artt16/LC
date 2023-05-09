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
        private readonly LCDbContext _db;

        public HomeController(LCDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var links = _db.Links.ToList();
            ViewBag.Links = links;
            ViewBag.ShortLink = ShortLink;
            ViewBag.FullLink = FullLink;
            return View();
        }

        public IActionResult Delete()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (id == null || id.Contains(" ") || id.Length != 9)
            {
                return RedirectToAction(nameof(Delete));
            }

            LinkModel link = _db.Links.Find(id);
            if (link != null)
            {
                _db.Links.Remove(link);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Create(string fullLink)
        {
            if (fullLink == null || fullLink.Contains(" "))
            {
                return RedirectToAction(nameof(Create));
            }

            var links = _db.Links.ToList();
            string id;
            const string shortLink = "http://lcs.biz/";

            do
            {
                id = RandomId();
            } while (links.Any(link => link.Id == id));

            FullLink = fullLink;
            ShortLink = shortLink + id;

            var linkModel = new LinkModel
            {
                Id = id,
                Full = fullLink,
                DateOfCreating = DateTime.Now,
                NumberOfTransitions = 0
            };

            _db.Links.Add(linkModel);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private static string RandomId()
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

        public static string? FullLink { get; set; }
        public static string? ShortLink { get; set; }
    }
}
