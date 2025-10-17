using Jorfofbasket.Data;
using Jorfofbasket.Models;
using Microsoft.AspNetCore.Mvc;

public class ClientController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClientController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult DashboardClient()
    {
        // Retourne la vue sans données Match
        return View();
    }

    public IActionResult Partners()
    {
        var culture = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        var partners = _context.Partners.ToList();

        var list = partners.Select(p => new {
            Title = culture == "fr" ? p.TitleFr : p.TitleEn,
            Country = p.Country,
            Photo = p.PhotoPath
        });

        return View(list);
    }
}
