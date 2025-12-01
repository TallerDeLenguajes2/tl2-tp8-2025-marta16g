using System;
using EspacioProducto;

namespace EspacioInterfaces
{
    public interface IProductoRepository
    {
        Producto GetById(int id);
        int Create(Producto producto);
        List<Producto> GetAll();
        void Update(int id, Producto producto);
        bool Delete(int id);
    }
}