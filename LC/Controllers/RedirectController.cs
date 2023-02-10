using LC.Data;
using LC.Data.Model;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Net.Mail;

namespace LC.Controllers
{
    public class RedirectController : Controller
    {
        public string RedirectToFullLink(string? url)
        {
            LCDbContext db = new LCDbContext();

            var id = string.IsNullOrEmpty(url) ? 
                throw new SmtpException((SmtpStatusCode)404, 
                "RedirectController don't cath the url") : 
                url.Split('/').Last();

            LinkModel link;
            string route = string.Empty;
            link = db.Links.First(link => link.Id == id);

            route = string.IsNullOrEmpty(link.Full) ?
                throw new SmtpException((SmtpStatusCode)404,
                "RedirectController: something wrong with link.Full") :
                link.Full;

            link.NumberOfTransitions += 1;

            db.Links.First(x => x.Id == id).NumberOfTransitions++;
            db.Links.Attach(link);
            db.SaveChanges();

            if (string.IsNullOrEmpty(route)) 
                throw new SmtpException((SmtpStatusCode)404, 
                    "Не получилось, не фартануло");

            return route;
        }
    }
}
