using System.Diagnostics;
using EspacioInterfaces;
using EspacioPresupuesto;
using EspacioProducto;
using EspacioRepositories;
using EspacioViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_tp8_2025_marta16g.Models;

namespace tl2_tp8_2025_marta16g.Controllers;

public class PresupuestoController : Controller
{
    private IPresupuestoRepository presupuestoRepository;
    private IProductoRepository productoRepository;
    private IAuthenticationService _authService;

    public PresupuestoController(IPresupuestoRepository presurepo, IProductoRepository prodRepo, IAuthenticationService authService)
    {
        presupuestoRepository = presurepo;
        productoRepository = prodRepo;
        _authService = authService;
    }

    public IActionResult Index()
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (_authService.HasAccessLevel("Administrador") ||
_authService.HasAccessLevel("Cliente"))
        {
            //si es es valido entra sino vuelve a login
            var presupuestos = presupuestoRepository.GetAll();
            return View(presupuestos);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult Detalle(int idPresupuesto)
    {
        Presupuesto miPresupuesto = presupuestoRepository.GetById(idPresupuesto);
        return View(miPresupuesto);
    }

    [HttpPost]
    public IActionResult AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }
        presupuestoRepository.AgregarDetalle(idPresupuesto, idProducto, cantidad);

        return RedirectToAction("Detalle", new { idPresupuesto });
    }


    [HttpGet]
    public IActionResult Editar(int idPresupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }
        Presupuesto presupuesto = presupuestoRepository.GetById(idPresupuesto);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Editar(Presupuesto presupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }
        presupuestoRepository.Update(presupuesto.IdPresupuesto, presupuesto);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Eliminar(int idPresupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }
        Presupuesto presupuesto = presupuestoRepository.GetById(idPresupuesto);
        return View(presupuesto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult EliminarConfirmado(int idPresupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }
        bool exito;
        exito = presupuestoRepository.Delete(idPresupuesto);
        if (exito)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return Error();
        }
    }

    [HttpGet]
    public IActionResult AgregarProducto(int idPresupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }
        List<Producto> productos = productoRepository.GetAll();

        AgregarProductoViewModel vm = new AgregarProductoViewModel { IdPresupuesto = idPresupuesto, ListaProductos = new SelectList(productos, "IdProducto", "Descripcion") };

        return View(vm);
    }


    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel vm)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        if (!ModelState.IsValid)
        {
            var productos = productoRepository.GetAll();
            vm.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            return View(vm);
        }

        presupuestoRepository.AgregarDetalle(vm.IdPresupuesto, vm.IdProducto, vm.Cantidad);

        return RedirectToAction(nameof(Detalle), new { idPresupuesto = vm.IdPresupuesto });
    }


    public IActionResult EliminarDetalle(int idPresupuesto, int idProducto)
    {
        presupuestoRepository.EliminarDetalle(idPresupuesto, idProducto);

        return RedirectToAction("Detalle", new { idPresupuesto = idPresupuesto });
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!_authService.HasAccessLevel("Administrador"))
        {
            // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
