using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_marta16g.Models;
using EspacioProductoRepository;
using EspacioProducto;
using EspacioViewModels;

namespace tl2_tp8_2025_marta16g.Controllers;

public class ProductoController : Controller
{
    private ProductoRepository productoRepository;

    public ProductoController()
    {
        productoRepository = new ProductoRepository();
    }

    public IActionResult Index()
    {
        List<Producto> listaDeProductos = productoRepository.ListarProductos();
        return View(listaDeProductos);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(ProductoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Producto nuevoProducto = new Producto { Descripcion = vm.Descripcion, Precio = vm.Precio };

        productoRepository.Alta(nuevoProducto);

        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Editar(int idProducto)
    {
        Producto producto = productoRepository.BuscarProducto(idProducto);

        ProductoViewModel nuevoVm = new ProductoViewModel { IdProducto = producto.IdProducto, Descripcion = producto.Descripcion, Precio = producto.Precio };

        return View(nuevoVm);
    }


    [HttpPost]
    public IActionResult Editar(ProductoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Producto productoEditado = new Producto{IdProducto = vm.IdProducto, Descripcion = vm.Descripcion, Precio = vm.Precio};

        productoRepository.Modificar(productoEditado.IdProducto, productoEditado);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int idProducto)
    {
        Producto producto = productoRepository.BuscarProducto(idProducto);
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult EliminarConfirmado(int idProducto)
    {
        bool exito;
        exito = productoRepository.EliminarProducto(idProducto);
        if (exito)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return Error();
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
