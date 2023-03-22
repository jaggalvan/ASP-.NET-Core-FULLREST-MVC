using AppBlogCore.Data;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {

        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Articulo articulo)
        {
            var objDesdeDb = _db.Articulo.FirstOrDefault(s => s.Id == articulo.Id);
            objDesdeDb.Nombre = articulo.Nombre;
            objDesdeDb.Descripcion = articulo.Descripcion;
            objDesdeDb.UrlImage = articulo.UrlImage;
            objDesdeDb.CategoriaId = articulo.CategoriaId;
           // _db.SaveChanges();
        }


    }
}
