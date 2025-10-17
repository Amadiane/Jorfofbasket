using Jorfofbasket.Data;
using Jorfofbasket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Ajouter la DbContext ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Ajouter Identity avec rôles et options de mot de passe ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// --- Configurer le cookie d'authentification ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Login";        // Redirection si non connecté
    options.AccessDeniedPath = "/Admin/Login"; // Redirection si pas autorisé
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

// --- Ajouter les services MVC / RazorPages ---
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Seed Super Admin ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedAdminAsync(services);
}

// --- Middleware ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // 🔑 Auth
app.UseAuthorization();  // 🔑 Roles et policies

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");

app.Run();


// ---------------- Seed Super Admin ----------------
static async Task SeedAdminAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // --- Crée le rôle Admin si inexistant ---
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // --- Vérifie si le Super Admin existe ---
    var superAdminEmail = "superadmin@example.com";
    var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

    if (superAdminUser == null)
    {
        superAdminUser = new ApplicationUser
        {
            UserName = superAdminEmail,
            Email = superAdminEmail
        };

        var result = await userManager.CreateAsync(superAdminUser, "Admin123!"); // Mot de passe par défaut
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(superAdminUser, "Admin");
        }
    }
}
