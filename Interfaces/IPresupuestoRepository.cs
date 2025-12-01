using System;
using EspacioPresupuesto;
using EspacioPresupuestoDetalle;

namespace EspacioInterfaces
{
    public interface IPresupuestoRepository
    {
        Presupuesto GetById(int id);
        int Create(Presupuesto presupuesto);
        List<Presupuesto> GetAll();
        void Update(int id, Presupuesto presupuesto);
        bool Delete(int id);
        List<PresupuestoDetalle> TraerDetallesPresupuesto(int id);
        public void AgregarDetalle(int idPresupuesto, int idProducto, int cantidad);
        void EliminarDetalle(int idPresupuesto, int idProducto);
    }
}