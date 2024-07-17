using Entidades;
using Logica_Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI_Invoicetics_Report.Controllers
{
    public class ProductoController : Controller
    {
        // Puente Con DB
        private readonly ProductoBL _ProductoBL;

        // Constructor: 
        public ProductoController(ProductoBL productoBL)
        {
            _ProductoBL = productoBL;
        }



        // Manda Todos Los Registros De La Tabla:
        public async Task<IActionResult> Registros_Productos()
        {
            List<Producto> Objetos_Obtenidos = await _ProductoBL.Obtener_Todos();

            return View(Objetos_Obtenidos);
        }


        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Detalle_Producto(int id)
        {
            Producto Objeto_Obtenido = await _ProductoBL.Obtener_PorId(new Producto() { IdProducto = id });

            return View(Objeto_Obtenido);
        }


        [AllowAnonymous]
        public IActionResult Registrar_Producto()
        {
            return View();
        }


        // Recibe Un Objeto Y Lo Guarda En La Tabla:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar_Producto(Producto producto)
        {
            await _ProductoBL.Registrar_Producto(producto);

            return RedirectToAction("Registros_Productos", "Producto");
        }


        // Manda Un Objeto Encontrado De La Tabla
        public async Task<IActionResult> Editar_Producto(int id)
        {
            Producto Objeto_Obtenido = await _ProductoBL.Obtener_PorId(new Producto() { IdProducto = id });

            return View(Objeto_Obtenido);
        }


        // Recibe El Objeto Que Fue Enviado Anteriormente:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Editar_Producto(Producto producto)
        {

            await _ProductoBL.Editar_Producto(producto);


            return RedirectToAction("Registros_Productos", "Producto");
        }


        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Eliminar_Producto(int id)
        {
            Producto Objeto_Obtenido = await _ProductoBL.Obtener_PorId(new Producto() { IdProducto = id });

            return View(Objeto_Obtenido);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Eliminar_Producto(Producto producto)
        {
            await _ProductoBL.Eliminar_Producto(producto);

            return RedirectToAction("Registros_Productos", "Producto");
        }


    }
}
