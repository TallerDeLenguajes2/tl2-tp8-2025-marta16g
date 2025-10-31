using System;
using EspacioProducto;
using Microsoft.AspNetCore.Mvc;


namespace EspacioProductoController
{
    [ApiController]
    [Route("api/Producto")]

    public class ProductoController : ControllerBase
    {
        private ProductoRepository productoRepository;
        public ProductoController()
        {
            productoRepository = new ProductoRepository();
        }

        [HttpPost("AltaProducto")]
        public IActionResult AltaProducto(Producto nuevoProducto)
        {
            int nuevoId = productoRepository.Alta(nuevoProducto);
            return Ok($"Producto dado de alta exitosamente con id: {nuevoId}");
        }

        [HttpPut("Producto/{id}")]
        public IActionResult Modificar(int id, Producto producto)
        {
            productoRepository.Modificar(id, producto);
            return Ok($"Producto de id {id} modificado");
        }
    }
}