using Entidades;
using Logica_Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI_Invoicetics_Report.Controllers
{
    public class FacturaController : Controller
    {
        // Puente Para La DB:
        private readonly FacturaBL _FacturaBL;

        // Constructor:
        public FacturaController(FacturaBL facturaBL)
        {
            _FacturaBL = facturaBL;
        }



        // Manda Todos Los Registros De La Tabla:
        public async Task<IActionResult> Registros_Facturas()
        {
            List<Factura> Objetos_Obtenidos = await _FacturaBL.Obtener_Todas();

            return View(Objetos_Obtenidos);
        }

        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Detalle_Factura(int id)
        {
            Factura Objeto_Obtenido = await _FacturaBL.Obtener_PorId(new Factura() { IdFactura = id });

            ViewBag.Accion = "Detalle_Factura";
            return View(Objeto_Obtenido);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Registrar_Factura()
        {
            // Contamos Los Registros:
            List<Factura> Objetos_Obtenidos = await _FacturaBL.Obtener_Todas();
            int Facturas_Registradas = Objetos_Obtenidos.Count;


            // Objeto Con Informacion de Inicio:
            Factura Objeto_Inicio = new Factura();

            DateTime fechaHoraActual = DateTime.Now;
            DateTime fechaHoraActualizada = new DateTime(
                fechaHoraActual.Year,
                fechaHoraActual.Month,
                fechaHoraActual.Day,
                fechaHoraActual.Hour,
                fechaHoraActual.Minute,
                0
            );
            Objeto_Inicio.FechaRealizada = fechaHoraActualizada;

            Objeto_Inicio.Correlativo = Facturas_Registradas + 1;

            Objeto_Inicio.Lista_DetalleFactura = new List<DetalleFactura>();
            Objeto_Inicio.Lista_DetalleFactura.Add(new DetalleFactura { CantidadComprada = 1, PrecioProducto = 0 });


            List<Empleado> Lista_Empleados = await _FacturaBL.Lista_Empleados();
            ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre");


            ViewBag.Accion = "Registrar_Factura";
            return View(Objeto_Inicio);
        }

        // Recibe Un Objeto Y Lo Guarda En La Tabla:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar_Factura(Factura factura)
        {
            // Verificar Si Un Atributo Del Detalle Biene Vasio
            if (factura.Lista_DetalleFactura.Any(x => x.NombreProducto == null || x.CantidadComprada<1 || x.PrecioProducto < 1))
            {

                if(factura.Lista_DetalleFactura.Any(x => x.NombreProducto ==null))
                {
                    TempData["ProductoRequerido"] = "Ingrese El Nombre Del Producto A LLevar.";
                }
                if (factura.Lista_DetalleFactura.Any(x => x.CantidadComprada <1))
                {
                    TempData["CantidadRequerida"] = "Ingrese La Cantidad A LLevar.";
                }
                if (factura.Lista_DetalleFactura.Any(x => x.PrecioProducto <1))
                {
                    TempData["PrecioRequerido"] = "Ingrese El Precio Del Producto.";
                }


                List<Empleado> Lista_Empleados = await _FacturaBL.Lista_Empleados();
                ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre", factura.IdEmpleadoEnFactura);


                ViewBag.Accion = "Registrar_Factura";
                return View(factura);
            }           
            else
            {
                factura.Total = factura.Lista_DetalleFactura.Sum(x => x.CantidadComprada * x.PrecioProducto);
                await _FacturaBL.Registrar_Factura(factura);
            }

            return RedirectToAction("Registros_Facturas", "Factura");
        }


        // Manda Un Objeto Encontrado De La Tabla
        public async Task<IActionResult> Editar_Factura(int id)
        {
            Factura Objeto_Obtenido = await _FacturaBL.Obtener_PorId(new Factura() { IdFactura = id });

            List<Empleado> Lista_Empleados = await _FacturaBL.Lista_Empleados();
            ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre", Objeto_Obtenido.IdEmpleadoEnFactura);

            ViewBag.Accion = "Editar_Factura";
            return View(Objeto_Obtenido);
        }

        // Recibe Un Objeto Y Lo Modifica En La Tabla:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Editar_Factura(Factura factura)
        {
            // Verificar Si Un Atributo Del Detalle Biene Vasio
            if (factura.Lista_DetalleFactura.Any(x => x.NombreProducto == null || x.CantidadComprada < 1 || x.PrecioProducto < 1))
            {

                if (factura.Lista_DetalleFactura.Any(x => x.NombreProducto == null))
                {
                    TempData["ProductoRequerido"] = "Ingrese El Nombre Del Producto A LLevar.";
                }
                if (factura.Lista_DetalleFactura.Any(x => x.CantidadComprada < 1))
                {
                    TempData["CantidadRequerida"] = "Ingrese La Cantidad A LLevar.";
                }
                if (factura.Lista_DetalleFactura.Any(x => x.PrecioProducto < 1))
                {
                    TempData["PrecioRequerido"] = "Ingrese El Precio Del Producto.";
                }


                List<Empleado> Lista_Empleados = await _FacturaBL.Lista_Empleados();
                ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre", factura.IdEmpleadoEnFactura);


                ViewBag.Accion = "Registrar_Factura";
                return View(factura);
            }
            else
            {
                await _FacturaBL.Editar_Factura(factura);
            }

            return RedirectToAction("Registros_Facturas", "Factura");
        }


        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Eliminar_Factura(int id)
        {
            Factura Objeto_Obtenido = await _FacturaBL.Obtener_PorId(new Factura() { IdFactura = id });

            ViewBag.Accion = "Eliminar_Factura";
            return View(Objeto_Obtenido);
        }


        // Recibe Un Objeto Y Lo Modifica En La Tabla:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Eliminar_Factura(Factura factura)
        {
            await _FacturaBL.Eliminar_Factura(factura);

            return RedirectToAction("Registros_Facturas", "Factura");
        }




        //         METODOS PARA AGREGAR O ELIMINAR DETALLES A LA FACTURA
        // **********************************************************************

        public async Task<IActionResult> Agregar_Detalle(Factura factura, string accion)
        {
            //Agregamos Detalle A La Lista:
            factura.Lista_DetalleFactura.Add(new DetalleFactura
            {
                CantidadComprada = 1,
                PrecioProducto = 0
            });

            List<Empleado> Lista_Empleados = await _FacturaBL.Lista_Empleados();
            ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre", factura.IdEmpleadoEnFactura);

            ViewBag.Accion = accion;
            return View(accion, factura);
        }


        public async Task<IActionResult> Eliminar_Detalle(Factura factura, int index, string accion)
        {
            DetalleFactura Objeto_Obtenido = factura.Lista_DetalleFactura[index];

            if (accion == "Editar_Factura" && Objeto_Obtenido.IdDetalleFactura > 0)
            {
                Objeto_Obtenido.IdDetalleFactura = Objeto_Obtenido.IdDetalleFactura * -1;
            }
            else
            {
                factura.Lista_DetalleFactura.RemoveAt(index);
            }

            List<Empleado> Lista_Empleados = await _FacturaBL.Lista_Empleados();
            ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre", factura.IdEmpleadoEnFactura);

            ViewBag.Accion = accion;
            return View(accion, factura);
        }

    }
}
