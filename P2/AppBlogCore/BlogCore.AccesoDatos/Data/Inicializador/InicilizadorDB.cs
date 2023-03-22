using AppBlogCore.Data;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Inicializador
{
    public class InicializadorDB : IInicializadorDB
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //Creamos el constructor
        public InicializadorDB(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public void Inicializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
            }

            if (_db.Roles.Any(ro => ro.Name == CNT.Admin)) return;

            //Creación de roles
            _roleManager.CreateAsync(new IdentityRole(CNT.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Usuario)).GetAwaiter().GetResult();


            //Creación del usuario inicial
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "jaggalvan@hotmail.com",
                Email = "jaggalvan@hotmail.com",
                EmailConfirmed = true,
                Nombre = "firstweb"
            }, "Purisima1*").GetAwaiter().GetResult();

            ApplicationUser usuario = _db.ApplicationUser
                .Where(us => us.Email == "jaggalvan@hotmail.com").FirstOrDefault();
                
            _userManager.AddToRoleAsync(usuario, CNT.Admin).GetAwaiter().GetResult();
        }
    }
}
