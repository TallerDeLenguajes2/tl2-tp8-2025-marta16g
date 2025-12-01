using System;
using EspacioUsuario;

namespace EspacioInterfaces
{
    public interface IUsuarioRepository
    {
        Usuario GetUser(string username, string password);
    }
}