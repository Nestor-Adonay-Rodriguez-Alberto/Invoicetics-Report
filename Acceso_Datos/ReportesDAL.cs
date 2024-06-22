using Entidades;
using Microsoft.EntityFrameworkCore;


namespace Acceso_Datos
{
    public class ReportesDAL
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;

        // Constructor:
        public ReportesDAL(MyDBcontext myDBcontext)
        {
            _MyDBcontext = myDBcontext;
        }



        // ******* METODOS QUE MANDARAN OBJETOS A LAS VISTAS ********
        // **********************************************************

        // Lista De La Tabla Empleados Para ViewData:
        public async Task<List<Empleado>> Lista_Empleados()
        {
            List<Empleado> Objetos_Obtenidos = await _MyDBcontext.Empleados.ToListAsync();

            return Objetos_Obtenidos;
        }


        // Manda Todos Los Registros De Tabla Facturas:
        public async Task<List<Factura>> Obtener_Facturas_Registradas()
        {
            List<Factura> Objetos_Obtenidos = await _MyDBcontext.Facturas
                .Include(x => x.Lista_DetalleFactura)
                .Include(x => x.Objeto_Empleado)
                .ToListAsync();

            return Objetos_Obtenidos;
        }
    }
}
