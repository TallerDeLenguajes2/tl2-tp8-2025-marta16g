using System.Diagnostics;
using EspacioPresupuesto;
using EspacioPresupuestoDetalle;
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
   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
