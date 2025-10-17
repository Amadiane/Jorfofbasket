using Microsoft.AspNetCore.Mvc;
using Jorfofbasket.Models;
using Jorfofbasket.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jorfofbasket.Controllers
{
    public class PartnersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Partners
        public async Task<IActionResult> Index()
        {
            var partners = await _context.Partners.ToListAsync();
            return View(partners); // retourne Partners.cshtml
        }

        // GET: /Partners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Partners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner partner, IFormFile Photo)
        {
            if (Photo != null && Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/partners");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Path.GetFileName(Photo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(stream);
                }

                partner.PhotoPath = "/images/partners/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(partner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(partner);
        }
    }
}
