using Entidades;
using Logica_Negocio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Rotativa.AspNetCore;

namespace UI_Invoicetics_Report.Controllers
{
    public class Reportes_PDF_EXCELController : Controller
    {
        // Puente Para La DB:
        private readonly ReportesBL _ReportesBL;

        // Constructor:
        public Reportes_PDF_EXCELController(ReportesBL reportesBL)
        {
            _ReportesBL = reportesBL;
        }



        // MANDA UNA LISTA DE EMPLEADOS:
        public async Task<IActionResult> Pagina_Inicio()
        {
            List<Empleado> Lista_Empleados = await _ReportesBL.Lista_Empleados();
            Lista_Empleados.Insert(0, new Empleado { IdEmpleado = 0, Nombre = "Seleccionar..." });

            ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre");

            return View();
        }


        // BUSCA LAS FACTURAS REGISTRADAS Y CREA UN REPORTE DE ELLAS:
        public async Task<IActionResult> Generar_Reporte(Factura factura, string Opcion)
        {
            // Validar Que Contenga Informacion factura:
            if (factura.IdEmpleadoEnFactura == 0 && factura.FechaBuscar == null && factura.MesBuscar == null)
            {
                TempData["Error"] = "Debe Ingresar Algun \"Empleado, Fecha o Meses\" Para Crear Un Reporte.";
                List<Empleado> Lista_Empleados = await _ReportesBL.Lista_Empleados();
                Lista_Empleados.Insert(0, new Empleado { IdEmpleado = 0, Nombre = "Seleccionar..." });

                ViewData["Lista_Empleados"] = new SelectList(Lista_Empleados, "IdEmpleado", "Nombre");

                return View("Pagina_Inicio");
            }
            // Facturas Encontradas:
            IQueryable<Factura> Objetos_Obtenidos = await Obtener_Facturas(factura);

            // CREACION DE LOS REPORTES:
            if (Opcion == "PDF")
            {

                return new ViewAsPdf("Reporte_PDF", Objetos_Obtenidos)
                {

                };
            }
            else if (Opcion == "EXCEL")
            {
                // Obtenemos El EXCEL:
                byte[] fileContents = await Crear_Excel(Objetos_Obtenidos);

                // Devolvemos el archivo Excel como una descarga
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EXCEL.xlsx");
            }
            else 
            {
                return Content("Selecciones El Tipo De Reporte Que Desea...");
            }
            
            
        }






        // OBTIENE LAS FACTURAS SEGUN LAS OPCIONES ELEGIDAS:
        private async Task<IQueryable<Factura>> Obtener_Facturas(Factura factura)
        {

            // Obtiene Todos Los Registros De Facturas:
            List<Factura> Objetos_Obtenidos = await _ReportesBL.Obtener_Facturas_Registradas();

            // Para Hacer Consultas
            IQueryable<Factura> Query_Objetos = Objetos_Obtenidos.AsQueryable();

            // Agrupamos Por Empleados:
            if (factura.IdEmpleadoEnFactura != 0)
            {
                Query_Objetos = Query_Objetos.Where(s => s.IdEmpleadoEnFactura == factura.IdEmpleadoEnFactura);
            }

            // Agrupamos Por Fechas:
            if (factura.FechaBuscar != null)
            {
                DateTime Fecha = factura.FechaBuscar.Value.Date;
                Query_Objetos = Query_Objetos.Where(s => s.FechaRealizada.Date == factura.FechaBuscar.Value.Date);
            }

            // Agrupamos Por Meses:
            if (!string.IsNullOrWhiteSpace(factura.MesBuscar))
            {
                IQueryable<Factura> Query_Objetos_2 = Facturas_Meses(Query_Objetos, factura);

                return Query_Objetos_2;
            }

            return Query_Objetos;
        }


        // SELECCIONA LAS FACTURAS QUE SOLO CUMPLAN CON LA CONDICION:
        private IQueryable<Factura> Facturas_Meses(IQueryable<Factura> Query_Objetos, Factura factura)
        {
            int Mes_Actual = DateTime.Now.Month;

            if (factura.MesBuscar == "Mes Actual.")
            {
                Query_Objetos = Query_Objetos.Where(s => s.FechaRealizada.Date.Month == Mes_Actual);
            }

            if (factura.MesBuscar == "Ultimos 3 Meses.")
            {
                int Mes_2 = Mes_Actual - 1;
                int Mes_3 = Mes_2 - 1;
                Query_Objetos = Query_Objetos.Where(s => s.FechaRealizada.Date.Month == Mes_Actual ||
                s.FechaRealizada.Date.Month == Mes_2 || s.FechaRealizada.Date.Month == Mes_3);
            }

            if (factura.MesBuscar == "Ultimos 6 Meses.")
            {
                int Mes_2 = Mes_Actual - 1;
                int Mes_3 = Mes_2 - 1;
                int Mes_4 = Mes_3 - 1;
                int Mes_5 = Mes_4 - 1;
                int Mes_6 = Mes_5 - 1;

                Query_Objetos = Query_Objetos.Where(s => s.FechaRealizada.Date.Month == Mes_Actual ||
                s.FechaRealizada.Date.Month == Mes_2 || s.FechaRealizada.Date.Month == Mes_3 ||
                s.FechaRealizada.Date.Month == Mes_4 || s.FechaRealizada.Date.Month == Mes_5 ||
                s.FechaRealizada.Date.Month == Mes_6);
            }

            return Query_Objetos;
        }

        //Crea El Excel
        private async Task<byte[]> Crear_Excel(IQueryable<Factura> Objetos_Obtenidos)
        {
            using (var package = new ExcelPackage())
            {
                // Agregamos una nueva hoja de trabajo al paquete
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reporte");
                // Establecer el ancho de las columnas
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 30;
                worksheet.Column(5).Width = 30;
                worksheet.Column(6).Width = 20;

                // Establecer estilo para los encabezados de columna
                var headerStyle = worksheet.Cells[1, 1, 1, 6].Style;
                headerStyle.Font.Bold = true;
                headerStyle.Font.Size = 11;
                headerStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Escribir encabezados de columna para datos de factura
                worksheet.Cells[1, 1].Value = "CORRELATIVO";
                worksheet.Cells[1, 2].Value = "FECHA REALIZADA";
                worksheet.Cells[1, 3].Value = "HORA REALIZADA";
                worksheet.Cells[1, 4].Value = "EMPLEADO";
                worksheet.Cells[1, 5].Value = "CLIENTE";
                worksheet.Cells[1, 6].Value = "TOTAL";

                int fila = 2;
                foreach (var item in Objetos_Obtenidos)
                {
                    string Fecha_Realizada = item.FechaRealizada.ToString("dd/MM/yyyy");
                    string Hora_Realizada = item.FechaRealizada.ToString("HH:mm tt");

                    // Recoriendo Los Datos De La Factura
                    worksheet.Cells[fila, 1].Value = item.Correlativo;
                    worksheet.Cells[fila, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[fila, 2].Value = Fecha_Realizada;
                    worksheet.Cells[fila, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[fila, 3].Value = Hora_Realizada;
                    worksheet.Cells[fila, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[fila, 4].Value = item.Objeto_Empleado.Nombre;
                    worksheet.Cells[fila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[fila, 5].Value = item.NombreCliente;
                    worksheet.Cells[fila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[fila, 6].Value = item.Total;
                    worksheet.Cells[fila, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int detalleFila = fila + 1;

                    // Establecer estilo para los encabezados de columna de los detalles de la factura
                    var productHeaderStyle = worksheet.Cells[detalleFila, 1, detalleFila, 3].Style;
                    productHeaderStyle.Font.Bold = true;
                    productHeaderStyle.Font.Size = 11;
                    productHeaderStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Escribir encabezados de columna para los detalles de list:
                    worksheet.Cells[detalleFila, 1].Value = "PRODUCTO";
                    worksheet.Cells[detalleFila, 2].Value = "CANTIDAD";
                    worksheet.Cells[detalleFila, 3].Value = "PRECIO";

                    detalleFila++;


                    // Recorriendo La Lista De Detalles De La Factura:
                    foreach (var itemDet in item.Lista_DetalleFactura)
                    {
                        worksheet.Cells[detalleFila, 1].Value = itemDet.Objeto_Producto.Nombre;
                        worksheet.Cells[detalleFila, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        worksheet.Cells[detalleFila, 2].Value = itemDet.CantidadComprada;
                        worksheet.Cells[detalleFila, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        worksheet.Cells[detalleFila, 3].Value = itemDet.PrecioProducto;
                        worksheet.Cells[detalleFila, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        detalleFila++;
                    }

                    fila = detalleFila + 1;
                }

                // Obtener el rango de celdas que contienen los datos
                var range = worksheet.Cells[1, 1, fila - 1, 6];

                // Agregar un filtro a ese rango
                range.AutoFilter = true;

                // Convertir el paquete a un array de bytes
                byte[] fileContents = package.GetAsByteArray();

                return fileContents;
            }

        }


    }
}
