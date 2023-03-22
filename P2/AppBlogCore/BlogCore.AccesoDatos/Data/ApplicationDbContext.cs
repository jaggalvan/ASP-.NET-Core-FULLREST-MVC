using BlogCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AppBlogCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Aquí se deben de agregar cada uno de los modelos
        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Articulo> Articulo { get; set; }

        public DbSet<Slider> Slider { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

    }
}
