using System;
using EspacioInterfaces;
using EspacioUsuario;
using Microsoft.Data.Sqlite;


namespace EspacioRepositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string connectionString = "Data Source=DB/MiTienda.db";
        public Usuario GetUser(string usuario, string contrasena)
        {
            Usuario user = null;

            const string consultaSQL = "SELECT idUsuario, Nombre, User, Pass, Rol FROM Usuario WHERE User = @Usuario AND Pass = @Contrasena";

            using var conexion = new SqliteConnection(connectionString);

            conexion.Open();

            using var comando = new SqliteCommand(consultaSQL, conexion);

            comando.Parameters.AddWithValue("@Usuario", usuario);
            comando.Parameters.AddWithValue("@Contrasena", contrasena);

            using var lector = comando.ExecuteReader();

            if (lector.Read())
            {
                user = new Usuario
                {
                    IdUsuario = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    User = lector.GetString(2),
                    Pass = lector.GetString(3),
                    Rol = lector.GetString(4)
                };

            }
            return user;
        }
    }
}