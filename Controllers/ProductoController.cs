using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_marta16g.Models;
using EspacioRepositories;
using EspacioProducto;
using EspacioViewModels;
using EspacioInterfaces;

namespace tl2_tp8_2025_marta16g.Controllers;

public class ProductoController : Controller
{
    private IProductoRepository productoRepository;
    private IAuthenticationService _authService;
    public ProductoController(IProductoRepository prodRepo,
IAuthenticationService authService)
    {
        // productoRepository = new ProductoRepository();
        productoRepository = prodRepo;
        _authService = authService;
    }

    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        List<Producto> listaDeProductos = productoRepository.GetAll();
        return View(listaDeProductos);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        return View();
    }

    [HttpPost]
    public IActionResult Crear(ProductoViewModel vm)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Producto nuevoProducto = new Producto { Descripcion = vm.Descripcion, Precio = vm.Precio };

        productoRepository.Create(nuevoProducto);

        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Editar(int idProducto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        Producto producto = productoRepository.GetById(idProducto);

        ProductoViewModel nuevoVm = new ProductoViewModel { IdProducto = producto.IdProducto, Descripcion = producto.Descripcion, Precio = producto.Precio };

        return View(nuevoVm);
    }


    [HttpPost]
    public IActionResult Editar(ProductoViewModel vm)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Producto productoEditado = new Producto { IdProducto = vm.IdProducto, Descripcion = vm.Descripcion, Precio = vm.Precio };

        productoRepository.Update(productoEditado.IdProducto, productoEditado);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int idProducto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        Producto producto = productoRepository.GetById(idProducto);
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult EliminarConfirmado(int idProducto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        bool exito;
        exito = productoRepository.Delete(idProducto);
        if (exito)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return Error();
        }
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
