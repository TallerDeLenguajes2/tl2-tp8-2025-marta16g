using System;
using Microsoft.Data.Sqlite;
using EspacioPresupuesto;
using EspacioPresupuestoDetalle;
using EspacioProducto;

public class PresupuestoRepository
{
    private string connectionString = "Data Source=DB/MiTienda.db";


    //CREAR UN NUEVO PRESUPUESTO
    public int AltaPresupuesto(Presupuesto presupuesto)
    {
        int nuevoId = 0;

        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            string consulta = "INSERT INTO Presupuesto (NombreDestinatario, FechaCreacion) VALUES(@Nombre, @Fecha)";

            using var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@Nombre", presupuesto.NombreDestinatario));
            comando.Parameters.Add(new SqliteParameter("@Fecha", presupuesto.FechaCreacion));

            nuevoId = Convert.ToInt32(comando.ExecuteScalar());

            return nuevoId;
        }
    }



    //LISTAR TODOS LOS PRESUPUESTOS
    public List<Presupuesto> ListarPresupuetos()
    {
        var presupuestos = new List<Presupuesto>();
        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            string consulta = "SELECT * FROM Presupuesto";
            using var comando = new SqliteCommand(consulta, conexion);

            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                var miPresupuesto = new Presupuesto(Convert.ToInt32(lector["IdPresupuesto"]),
                lector["NombreDestinatario"].ToString(),
                Convert.ToDateTime(lector["FechaCreacion"]),
                TraerDetallesPresupuesto(Convert.ToInt32(lector["IdPresupuesto"])));

                presupuestos.Add(miPresupuesto);
            }
        }
        return presupuestos;
    }

    //OBTENER DETALLES DE UN PRESUPUESTO
    public List<PresupuestoDetalle> TraerDetallesPresupuesto(int id)
    {
        var listaDetalles = new List<PresupuestoDetalle>();
        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            string consulta = "select * from Presupuesto inner join PresupuestoDetalle using(IdPresupuesto) inner join Producto using(IdProducto) WHERE IdPresupuesto = @Id";

            var comando = new SqliteCommand(consulta, conexion);
            comando.Parameters.Add(new SqliteParameter("@Id", id));

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var producto = new Producto(Convert.ToInt32(lector["IdProducto"]), lector["Descripcion"].ToString(), Convert.ToDouble(lector["Precio"]));

                var detalle = new PresupuestoDetalle(producto, Convert.ToInt32(lector["Cantidad"]));

                listaDetalles.Add(detalle);
            }

        }
        return listaDetalles;
    }


    //AGREGAR UN PRODUCTO Y CANTIDAD A UN PRESUPUESTO
    public bool AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            string consulta = "INSERT INTO PresupuestoDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@IdPre, @IdPro, @Cant)";

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@IdPre", idPresupuesto));
            comando.Parameters.Add(new SqliteParameter("@IdPro", idProducto));
            comando.Parameters.Add(new SqliteParameter("@Cant", cantidad));

            int filas = comando.ExecuteNonQuery();
            if (filas > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    //ELIMINAR UN PRESUPUESTO POR ID
    public bool EliminarPresupuesto(int id)
    {
        using (var conexion = new SqliteConnection(connectionString))
        {
            conexion.Open();
            
            string consulta = "DELETE FROM Presupuesto WHERE IdPresupuesto = @Id";

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@Id", id));

            int filas = comando.ExecuteNonQuery();
            if (filas > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}