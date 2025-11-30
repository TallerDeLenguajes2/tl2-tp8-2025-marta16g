using System.Diagnostics;
using EspacioPresupuesto;
using EspacioPresupuestoDetalle;
using EspacioProducto;
using EspacioProductoRepository;
using EspacioViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_tp8_2025_marta16g.Models;

namespace tl2_tp8_2025_marta16g.Controllers;

public class PresupuestoController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    private ProductoRepository productoRepository;

    public PresupuestoController()
    {
        presupuestoRepository = new PresupuestoRepository();
        productoRepository = new ProductoRepository();
    }

    public IActionResult Index()
    {
        List<Presupuesto> listaDePresupuestos;
        listaDePresupuestos = presupuestoRepository.ListarPresupuetos();
        return View(listaDePresupuestos);
    }

    [HttpGet]
    public IActionResult Detalle(int idPresupuesto)
    {
        Presupuesto miPresupuesto = presupuestoRepository.BuscarPresupuesto(idPresupuesto);
        return View(miPresupuesto);
    }

    [HttpPost]
    public IActionResult AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        presupuestoRepository.AgregarDetalle(idPresupuesto, idProducto, cantidad);

        return RedirectToAction("Detalle", new { idPresupuesto });
    }


    [HttpGet]
    public IActionResult Editar(int idPresupuesto)
    {
        Presupuesto presupuesto = presupuestoRepository.BuscarPresupuesto(idPresupuesto);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Editar(Presupuesto presupuesto)
    {
        presupuestoRepository.Modificar(presupuesto.IdPresupuesto, presupuesto);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Eliminar(int idPresupuesto)
    {
        Presupuesto presupuesto = presupuestoRepository.BuscarPresupuesto(idPresupuesto);
        return View(presupuesto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult EliminarConfirmado(int idPresupuesto)
    {
        bool exito;
        exito = presupuestoRepository.EliminarPresupuesto(idPresupuesto);
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
        List<Producto> productos = productoRepository.ListarProductos();

        AgregarProductoViewModel vm = new AgregarProductoViewModel { IdPresupuesto = idPresupuesto, ListaProductos = new SelectList(productos, "IdProducto", "Descripcion") };

        return View(vm);
    }


    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel vm)
    {
        
        if (!ModelState.IsValid)
        {
            var productos = productoRepository.ListarProductos();
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
