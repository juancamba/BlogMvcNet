using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        // el no los llamo con la coletilla Repository, los puso como CAtegoria y Articulo
        public ICategoriaRepository _categoriaRepository { get; private set; }
        public IArticuloRepository _articuloRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            _categoriaRepository = new CategoriaRepository(_db);
            _articuloRepository = new ArticuloRepository(_db);
        }



        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
