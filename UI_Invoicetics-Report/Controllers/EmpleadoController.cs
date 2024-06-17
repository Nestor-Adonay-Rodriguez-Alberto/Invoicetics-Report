using Entidades;
using Logica_Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI_Invoicetics_Report.Controllers
{
    public class EmpleadoController : Controller
    {
        // Puente Para La DB:
        private readonly EmpleadoBL _EmpleadoBL;


        // Constructor:
        public EmpleadoController(EmpleadoBL empleadoBL)
        {
            _EmpleadoBL = empleadoBL;
        }




        // Manda Todos Los Registros De La Tabla:
        public async Task<IActionResult> Registros_Empleados()
        {
            List<Empleado> Objetos_Obtenidos = await _EmpleadoBL.Obtener_Todos();

            return View(Objetos_Obtenidos);
        }


        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Detalle_Empleado(int id)
        {
            Empleado Objeto_Obtenido = await _EmpleadoBL.Obtener_PorId(new Empleado() { IdEmpleado = id });

            return View(Objeto_Obtenido);
        }


        [AllowAnonymous]
        public IActionResult Registrar_Empleado()
        {
            return View();
        }


        // Recibe Un Objeto Y Lo Guarda En La Tabla:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar_Empleado(Empleado empleado, IFormFile Fotografia)
        {
            if(Fotografia!=null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Fotografia.CopyTo(memoryStream);

                    empleado.Fotografia = memoryStream.ToArray();
                }
            }

            await _EmpleadoBL.RegistrarEmpleado(empleado);

            return RedirectToAction("Index", "Home");
        }

        // Manda Un Objeto Encontrado De La Tabla
        public async Task<IActionResult> Editar_Empleado(int id)
        {
            Empleado Objeto_Obtenido = await _EmpleadoBL.Obtener_PorId(new Empleado() { IdEmpleado = id });

            return View(Objeto_Obtenido);
        }


        // Recibe El Objeto Que Fue Enviado Anteriormente:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Editar_Empleado(Empleado empleado, IFormFile Fotografia)
        {
            Empleado Objeto_Obtenido = await _EmpleadoBL.Obtener_PorId(new Empleado() { IdEmpleado = empleado.IdEmpleado });

            // Convirtiendo a Arreglo De Bytes:
            if (Fotografia != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Fotografia.CopyTo(memoryStream);

                    empleado.Fotografia = memoryStream.ToArray();
                }
            }
            else
            {
                empleado.Fotografia = Objeto_Obtenido.Fotografia;
            }

            await _EmpleadoBL.EditarEmpleado(empleado);


            return RedirectToAction("Index", "Home");
        }


        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Eliminar_Empleado(int id)
        {
            Empleado Objeto_Obtenido = await _EmpleadoBL.Obtener_PorId(new Empleado() { IdEmpleado = id });

            return View(Objeto_Obtenido);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Eliminar_Empleado(Empleado empleado)
        {
            await _EmpleadoBL.EliminarEmpleado(empleado);

            return RedirectToAction("Index", "Home");
        }
    }
}
