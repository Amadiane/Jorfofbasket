using Jorfofbasket.Data;
using Jorfofbasket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jorfofbasket.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Formulaire Contact (Index)
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactMessage message)
        {
            if (ModelState.IsValid)
            {
                _context.ContactMessages.Add(message);
                _context.SaveChanges();
                TempData["Message"] = "Votre message a été envoyé avec succès !";
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // Dashboard Admin
        public IActionResult Dashboard()
        {
            var messages = _context.ContactMessages.ToList();
            return View(messages);
        }
    }
}
