using System;
using EspacioPresupuestoDetalle;

namespace EspacioPresupuesto
{

    public class Presupuesto
    {
        private int idPresupuesto;
        private string nombreDestinatario;
        private DateTime fechaCreacion;
        private List<PresupuestoDetalle> detalle = new List<PresupuestoDetalle>();

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

        public Presupuesto(){}
        public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalle)
        {
            this.idPresupuesto = idPresupuesto;
            this.nombreDestinatario = nombreDestinatario;
            this.fechaCreacion = fechaCreacion;
            this.detalle = detalle;
        }

        public double MontoPresupuesto()
        {
            double total = 0;
            foreach (PresupuestoDetalle d in Detalle)
            {
                total += d.Producto.Precio * d.Cantidad;
            }
            return total;
        }

        public double MontoPresupuestoConIva()
        {
            double total = 0;
            foreach (var d in Detalle)
            {
                total += d.Producto.Precio * d.Cantidad;
            }
            return total * 1.2;
        }

        public int CantidadProductos()
        {
            int totalProductos = 0;
            foreach (var d in Detalle)
            {
                totalProductos += d.Cantidad;
            }
            return totalProductos;
        }
    }
}