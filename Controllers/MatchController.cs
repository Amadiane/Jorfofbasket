using Jorfofbasket.Data;
using Jorfofbasket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jorfofbasket.Controllers
{
    // [Authorize(Roles = "Admin")] // 🔐 Seul l’admin peut gérer les matchs
    // [Authorize] // juste connecter pour accéder
    public class MatchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🟢 Liste des matchs
        public async Task<IActionResult> Index()
        {
            var matches = await _context.Matches.OrderBy(m => m.DateMatch).ToListAsync();
            return View(matches);
        }

        // 🟢 Détails d’un match
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var match = await _context.Matches.FindAsync(id);
            if (match == null) return NotFound();
            return View(match);
        }

        // 🟢 Créer un match
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(Match match)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(match);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(match);
        // }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Match match)
        {
            if (!ModelState.IsValid)
            {
                // Affiche toutes les erreurs
                var errors = ModelState
                                .Where(ms => ms.Value.Errors.Count > 0)
                                .Select(ms => new { Field = ms.Key, Errors = ms.Value.Errors.Select(e => e.ErrorMessage) })
                                .ToList();

                foreach (var err in errors)
                {
                    Console.WriteLine($"Champ: {err.Field}, Erreurs: {string.Join(", ", err.Errors)}");
                }

                return View(match); // retourne la vue avec les erreurs
            }

            _context.Add(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // 🟢 Modifier un match
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var match = await _context.Matches.FindAsync(id);
            if (match == null) return NotFound();
            return View(match);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Match match)
        {
            if (id != match.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // 🟢 Supprimer un match
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var match = await _context.Matches.FindAsync(id);
            if (match == null) return NotFound();
            return View(match);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

