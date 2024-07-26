using Acceso_Datos;
using Entidades;


namespace Logica_Negocio
{
    public class FacturaBL
    {
        // Objeto De La DB:
        private readonly FacturaDAL _FacturaDAL;


        // Constructor:
        public FacturaBL(FacturaDAL facturaDAL)
        {
            _FacturaDAL= facturaDAL;
        }




        // ******* METODOS QUE MANDARAN OBJETOS A LAS VISTAS ********
        // **********************************************************

        // Manda Un Objeto Encontrado:
        public async Task<Factura> Obtener_PorId(Factura factura)
        {
            return await _FacturaDAL.Obtener_PorId(factura);
        }


        // Manda Todos Los Objetos:
        public async Task<List<Factura>> Obtener_Todas()
        {
            return await _FacturaDAL.Obtener_Todas();
        }


        // Lista De La Tabla Empleado Para ViewData:
        public async Task<List<Empleado>> Lista_Empleados()
        {
            return await _FacturaDAL.Lista_Empleados();
        }

        // Lista De La Tabla Producto Para ViewData:
        public async Task<List<Producto>> Lista_Productos()
        {
            return await _FacturaDAL.Lista_Productos();
        }

        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  ********
        // ********************************************************************

        // Recibe Un Objeto Lo Guarda En La DB:
        public async Task<int> Registrar_Factura(Factura factura)
        {
            return await _FacturaDAL.Registrar_Factura(factura);
        }


        // Recibe Un Objeto Lo Busca Y Modifica El Encontrado Con El Nuevo:
        public async Task<int> Editar_Factura(Factura factura)
        {
            return await _FacturaDAL.Editar_Factura(factura);
        }


        // Recibe Un Objeto Lo Busca Y Elimina El Encontrado:
        public async Task<int> Eliminar_Factura(Factura factura)
        {
            return await _FacturaDAL.Eliminar_Factura(factura);
        }
    }
}
