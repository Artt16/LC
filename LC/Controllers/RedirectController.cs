using LC.Data;
using LC.Data.Model;
using LC.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LC.Controllers
{
    [Route("{shortLink}")]
    public class RedirectController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly LCDbContext _db;

        public RedirectController(IConfiguration configuration, LCDbContext db)
        {
            _configuration = configuration;
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> RedirectToLongLink(string shortLink)
        {
            var longLink = await _db.Links
                .Where(l => shortLink.Contains(l.Id))
                .Select(l => l.Full)
                .FirstOrDefaultAsync();

            if (longLink == null)
            {
                return NotFound();
            }

            return Redirect(longLink);
            }
        }
    }
