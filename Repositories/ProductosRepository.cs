using System;
using Microsoft.AspNetCore.Mvc;
using EspacioProducto;
using Microsoft.Data.Sqlite;

namespace EspacioProductoRepository
{

    public class ProductoRepository
    {
        private string connectionString = "Data Source=DB/MiTienda.db";

        //TRAER UN PRODUCTO POR ID
        public Producto BuscarProducto(int id)
        {
            using (var conexion = new SqliteConnection(connectionString))
            {
                var miProducto = new Producto();
                conexion.Open();
                string consulta = "SELECT * FROM Producto WHERE IdProducto = @Id";
                using var comando = new SqliteCommand(consulta, conexion);
                comando.Parameters.Add(new SqliteParameter("@Id", id));

                using var lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    miProducto = new Producto(Convert.ToInt32(lector["IdProducto"]),
                    lector["Descripcion"].ToString(), Convert.ToDouble(lector["Precio"]));
                }

                return miProducto;
            }
        }
        public int Alta(Producto producto)
        {
            int nuevoId = 0;
            using (var conexion = new SqliteConnection(connectionString))
            {
                conexion.Open();
                string consulta = "INSERT INTO Producto (Descripcion, Precio) VALUES(@Descripcion, @Precio); SELECT last_insert_rowid();";
                using var comando = new SqliteCommand(consulta, conexion);
                comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
                comando.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
                nuevoId = Convert.ToInt32(comando.ExecuteScalar());
                // comando.ExecuteNonQuery();
            }
            return nuevoId;
        }

        public void Modificar(int id, Producto producto)
        {
            using (var conexion = new SqliteConnection(connectionString))
            {
                conexion.Open();
                string consulta = "UPDATE Producto SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @Id";
                using var comando = new SqliteCommand(consulta, conexion);
                comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
                comando.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
                comando.Parameters.Add(new SqliteParameter("@Id", id));

                comando.ExecuteNonQuery();
            }
        }

        public List<Producto> ListarProductos()
        {
            var productos = new List<Producto>();
            using (var conexion = new SqliteConnection(connectionString))
            {
                conexion.Open();
                string consulta = "SELECT * FROM Producto";
                using var comando = new SqliteCommand(consulta, conexion);

                using var lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    var miProducto = new Producto(Convert.ToInt32(lector["idProducto"]), lector["Descripcion"].ToString(), Convert.ToDouble(lector["Precio"]));
                    productos.Add(miProducto);
                }
            }
            return productos;
        }

        public bool EliminarProducto(int id)
        {
            using (var conexion = new SqliteConnection(connectionString))
            {
                conexion.Open();

                string consulta = "DELETE FROM Producto WHERE IdProducto = @Id";

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


}