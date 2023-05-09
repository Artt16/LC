using LC.Data;
using LC.Data.Model;
using LC.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace LC.Controllers
{
    [Route("{shortLink}")]
    public class RedirectController : Controller
    {
        private readonly IConfiguration _configuration;

        public RedirectController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<IActionResult> RedirectToLongLink(string shortLink)
        {
            string longLink = await GetLongLinkFromDatabase(shortLink);

            if (string.IsNullOrEmpty(longLink))
            {
                return NotFound();
            }

            return Redirect(longLink);
        }

        private async Task<string> GetLongLinkFromDatabase(string shortLink)
        {
            using (var db = new LCDbContext())
            {
                var id = string.IsNullOrEmpty(shortLink) ?
                throw new NotFoundException("RedirectController: URL не был передан") :
                shortLink.Split('/').
                LastOrDefault(s => !string.IsNullOrEmpty(s)) ?? 
                throw new NotFoundException("RedirectController: короткая ссылка неверна");

                var currentLink = await db.Links.FindAsync(id);
                if (currentLink == null)
                {
                    throw new NotFoundException(
                        "RedirectController: ссылка не найдена в базе данных");
                }

                currentLink.NumberOfTransitions++;
                await db.SaveChangesAsync();

                if (string.IsNullOrEmpty(currentLink.Full))
                {
                    throw new NotFoundException(
                        "RedirectController: некорректная ссылка в базе данных");
                }

                return currentLink.Full.ToString();
            }
        }
    }
}
