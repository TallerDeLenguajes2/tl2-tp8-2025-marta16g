using System.Diagnostics;
using EspacioPresupuesto;
using EspacioPresupuestoDetalle;
using EspacioProducto;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_marta16g.Models;

namespace tl2_tp8_2025_marta16g.Controllers;

public class PresupuestoController : Controller
{
    private PresupuestoRepository presupuestoRepository;

    public PresupuestoController()
    {
        presupuestoRepository = new PresupuestoRepository();
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
        List<PresupuestoDetalle> detalle;
        detalle = presupuestoRepository.TraerDetallesPresupuesto(idPresupuesto);
        return View(detalle);
    }

    [HttpPost]
    public IActionResult AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        bool exito;
        exito = presupuestoRepository.AgregarDetalle(idPresupuesto, idProducto, cantidad);
        if (exito)
        {
            return RedirectToAction("Detalle");
        }
        else
        {
            return BadRequest("Algo salió mal");
        }
    }


    [HttpGet]
    public IActionResult Eliminar(int idPresupuesto)
    {
        List<Presupuesto> listaDePresupuestos = presupuestoRepository.ListarPresupuetos();
        Presupuesto presupuestoAEliminar = listaDePresupuestos.Find(p => p.IdPresupuesto == idPresupuesto);
        return View(presupuestoAEliminar);
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


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
