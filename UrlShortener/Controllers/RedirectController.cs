using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RedirectController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<IActionResult> RedirectRequest(string code)
        {
            var shortenedUrl = await _dbContext.ShortenedUrls
                .FirstOrDefaultAsync(s => s.Code == code);

            if (shortenedUrl is null)
            {
                return NotFound();
            }

            return Redirect(shortenedUrl.LongUrl);
        }
    }
}
