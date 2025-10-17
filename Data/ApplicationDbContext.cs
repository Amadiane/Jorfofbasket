using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Jorfofbasket.Models;

namespace Jorfofbasket.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
       
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Partner> Partners { get; set; }
    
    }
}
