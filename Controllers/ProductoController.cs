using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_marta16g.Models;
using EspacioProductoRepository;
using EspacioProducto;

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
    public IActionResult Editar(int idProducto)
    {
        Producto producto = productoRepository.BuscarProducto(idProducto);
        return View(producto);
    }


    [HttpPost]
    public IActionResult Editar(Producto producto)
    {
        productoRepository.Modificar(producto.IdProducto, producto);

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
