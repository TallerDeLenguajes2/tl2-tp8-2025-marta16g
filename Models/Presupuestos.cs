using System;
using EspacioPresupuestoDetalle;

namespace EspacioPresupuesto
{

    class Presupuesto
    {
        private int idPresupuesto;
        private string nombreDestinatario;
        private DateTime fechaCreacion;
        private List<PresupuestoDetalle> detalle;

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

        public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestoDetalle> detalle)
        {
            this.idPresupuesto = idPresupuesto;
            this.nombreDestinatario = nombreDestinatario;
            this.fechaCreacion = fechaCreacion;
            this.detalle = detalle;
        }

        public float MontoPresupuesto()
        {
            return 0;
        }

        public float MontoPresupuestoConIva()
        {
            return 0;
        }

        public int CantidadProductos()
        {
            return 0;
        }
    }
}