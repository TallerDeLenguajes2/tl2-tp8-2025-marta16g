using System;
using Microsoft.Data.Sqlite;
using EspacioPresupuesto;
using EspacioPresupuestoDetalle;
using EspacioProducto;
using EspacioInterfaces;

namespace EspacioRepositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private string connectionString = "Data Source=DB/MiTienda.db";

        //TRAER UN PRESUPUESTO POR ID
        public Presupuesto GetById(int id)
        {
            using (var conexion = new SqliteConnection(connectionString))
            {
                var miPresupuesto = new Presupuesto();
                conexion.Open();
                string consulta = "SELECT * FROM Presupuesto WHERE IdPresupuesto = @Id";
                using var comando = new SqliteCommand(consulta, conexion);
                comando.Parameters.Add(new SqliteParameter("@Id", id));

                using var lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    miPresupuesto = new Presupuesto(Convert.ToInt32(lector["IdPresupuesto"]),
                    lector["NombreDestinatario"].ToString(),
                    Convert.ToDateTime(lector["FechaCreacion"]),
                    TraerDetallesPresupuesto(Convert.ToInt32(lector["IdPresupuesto"])));
                }

                return miPresupuesto;


            }
        }


        //CREAR UN NUEVO PRESUPUESTO
        public int Create(Presupuesto presupuesto)
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
        public List<Presupuesto> GetAll()
        {
            var presupuestos = new List<Presupuesto>();
            using (var conexion = new SqliteConnection(connectionString))
            {
                conexion.Open();
                string consulta = "SELECT * FROM Presupuesto ORDER BY FechaCreacion";
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

        //MODIFICAR UN PRESUPUESTO
        public void Update(int id, Presupuesto presupuesto)
        {
            using (var conexion = new SqliteConnection(connectionString))
            {
                conexion.Open();
                string consulta = "UPDATE Presupuesto SET NombreDestinatario = @Nombre, FechaCreacion = @Fecha WHERE idPresupuesto = @Id";
                using var comando = new SqliteCommand(consulta, conexion);
                comando.Parameters.Add(new SqliteParameter("@Nombre", presupuesto.NombreDestinatario));
                comando.Parameters.Add(new SqliteParameter("@Fecha", presupuesto.FechaCreacion));
                comando.Parameters.Add(new SqliteParameter("@Id", id));

                comando.ExecuteNonQuery();
            }
        }


        //ELIMINAR UN PRESUPUESTO POR ID
        public bool Delete(int id)
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
        public void AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
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
                // if (filas > 0)
                // {
                //     return true;
                // }
                // else
                // {
                //     return false;
                // }
            }
        }
        public void EliminarDetalle(int idPresupuesto, int idProducto)
        {
            using (SqliteConnection conexion = new SqliteConnection(connectionString))
            {
                conexion.Open();
                string consulta = "DELETE FROM PresupuestoDetalle WHERE idPresupuesto = @idPres AND idProducto = @idProd";

                using var deleteCmd = new SqliteCommand(consulta, conexion);
                deleteCmd.Parameters.Add(new SqliteParameter("@idPres", idPresupuesto));
                deleteCmd.Parameters.Add(new SqliteParameter("@idProd", idProducto));

                deleteCmd.ExecuteNonQuery();
                conexion.Close();
            }
        }

    }
}