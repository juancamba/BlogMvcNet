using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {


        ICategoriaRepository _categoriaRepository { get; }
        //aqui van todos los repositorios
        IArticuloRepository _articuloRepository { get; }
        void Save();



    }
}