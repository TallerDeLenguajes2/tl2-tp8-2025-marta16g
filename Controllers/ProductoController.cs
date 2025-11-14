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



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
