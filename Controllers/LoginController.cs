using System;
using EspacioInterfaces;
using EspacioViewModels;
using Microsoft.AspNetCore.Mvc;


public class LoginController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public IActionResult Index()
    {
        var vm = new LoginViewModel
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
        };
        return View(vm);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel vm)
    {

        if (string.IsNullOrEmpty(vm.Username) || string.IsNullOrEmpty(vm.Pass))
        {
            vm.ErrorMessage = "Debe ingresar usuario y contraseña.";
            return View("Index", vm);
        }

        if (_authenticationService.Login(vm.Username, vm.Pass))
        {
            return RedirectToAction("Index", "Home");
        }

        vm.ErrorMessage = "Credenciales inválidas.";
        vm.IsAuthenticated = false;
        return View("Index", vm);
    }

     public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la vista de login
        return RedirectToAction("Index");
    }
}