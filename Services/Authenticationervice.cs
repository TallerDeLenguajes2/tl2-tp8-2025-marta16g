using System;
using EspacioInterfaces;

namespace EspacioServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private HttpContext context;

        public AuthenticationService(IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;
            context = _httpContextAccessor.HttpContext;
        }

        public bool Login(string username, string password)
        {
            var user = _usuarioRepository.GetUser(username, password);

            if (user != null)
            {
                if (context == null)
                {
                    throw new InvalidOperationException("HttpContext no está disponible");
                }

                context.Session.SetString("IsAuthenticated", "true");
                context.Session.SetString("User", user.User);
                context.Session.SetString("Nombre", user.Nombre);
                context.Session.SetString("Rol", user.Rol);

                return true;
            }

            return false;
        }


        public void Logout()
        {
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible");
            }

            context.Session.Clear();
        }

        public bool IsAuthenticated()
        {
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible");
            }

            return context.Session.GetString("IsAuthenticated") == "true";
        }

        public bool HasAccessLevel(string requiredAccessLevel)
        {
             if(context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible");
            }
            return context.Session.GetString("Rol") == requiredAccessLevel;
        }
    }
}