using Entidades;
using Logica_Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace UI_Invoicetics_Report.Controllers
{
    public class GraficosController : Controller
    {
        // Puente Entre DB:
        private readonly FacturaBL _FacturaBL;

        // Constructor:
        public GraficosController(FacturaBL facturaBL)
        {
            _FacturaBL = facturaBL;
        }


        // MUESTRA LA VISTA CON LOS GRAFICOS:
        public IActionResult Graficos_Facturas()
        {
            return View();
        }


        // MANDA UN Json DE LAS FACTURAS REGISTRADAS POR CADA MES EN 6 MESES:
        public async Task<ActionResult> Obtener_Facturas()
        {

            // Facturas Registradas:
            List<Factura> Objetos_Obtenidos = await _FacturaBL.Obtener_Todas();

            // MESES:
            int Mes_6 = DateTime.Now.Month;
            int Mes_5 = Mes_6 - 1;
            if (Mes_5 == 0)
            {
                Mes_5 = 12;
            }
            int Mes_4 = Mes_5 - 1;
            if (Mes_4 == 0)
            {
                Mes_4 = 12;
            }
            int Mes_3 = Mes_4 - 1;
            if (Mes_3 == 0)
            {
                Mes_3 = 12;
            }
            int Mes_2 = Mes_3 - 1;
            if (Mes_2 == 0)
            {
                Mes_2 = 12;
            }
            int Mes_1 = Mes_2 - 1;
            if (Mes_1 == 0)
            {
                Mes_1 = 12;
            }

            List<Factura> Facturas_Mes6 = Objetos_Obtenidos.Where(x => x.FechaRealizada.Date.Month == Mes_6).ToList();
            List<Factura> Facturas_Mes5 = Objetos_Obtenidos.Where(x => x.FechaRealizada.Date.Month == Mes_5).ToList();
            List<Factura> Facturas_Mes4 = Objetos_Obtenidos.Where(x => x.FechaRealizada.Date.Month == Mes_4).ToList();
            List<Factura> Facturas_Mes3 = Objetos_Obtenidos.Where(x => x.FechaRealizada.Date.Month == Mes_3).ToList();
            List<Factura> Facturas_Mes2 = Objetos_Obtenidos.Where(x => x.FechaRealizada.Date.Month == Mes_2).ToList();
            List<Factura> Facturas_Mes1 = Objetos_Obtenidos.Where(x => x.FechaRealizada.Date.Month == Mes_1).ToList();


            // Lista De Todas Las Facturas Encontradas
            List<object> Lista_Facturas = new List<object>();

            Lista_Facturas.Add(new
            {
                // Mes Buscado:
                grupo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes_6),

                // Facturas Obtenidas:
                cantidad = Facturas_Mes6.Count()
            });

            Lista_Facturas.Add(new
            {
                // Mes Buscado:
                grupo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes_5),

                // Facturas Obtenidas:
                cantidad = Facturas_Mes5.Count()
            });

            Lista_Facturas.Add(new
            {
                // Mes Buscado:
                grupo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes_4),

                // Facturas Obtenidas:
                cantidad = Facturas_Mes4.Count()
            });

            Lista_Facturas.Add(new
            {
                // Mes Buscado:
                grupo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes_3),

                // Facturas Obtenidas:
                cantidad = Facturas_Mes3.Count()
            });

            Lista_Facturas.Add(new
            {
                // Mes Buscado:
                grupo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes_2),

                // Facturas Obtenidas:
                cantidad = Facturas_Mes2.Count()
            });

            Lista_Facturas.Add(new
            {
                // Mes Buscado:
                grupo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes_1),

                // Facturas Obtenidas:
                Cantidad = Facturas_Mes1.Count()
            });


            return Json(Lista_Facturas);
        }
    }
}
