using Acceso_Datos;
using Entidades;


namespace Logica_Negocio
{
    public class ProductoBL
    {
        // Objeto De La DB:
        private readonly ProductoDAL _ProductoDAL;

        // Constructor:
        public ProductoBL(ProductoDAL productoDAL)
        {
            _ProductoDAL = productoDAL;
        }



        // ******* METODOS QUE MANDARAN OBJETOS A LAS VISTAS ********
        // **********************************************************

        // Manda Un Objeto Encontrado:
        public async Task<Producto> Obtener_PorId(Producto producto)
        {
            return await _ProductoDAL.Obtener_PorId(producto);
        }

        // Manda Todos Los Objetos:
        public async Task<List<Producto>> Obtener_Todos()
        {
            return await _ProductoDAL.Obtener_Todos();
        }



        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  ********
        // ********************************************************************

        // Recibe Un Objeto Lo Guarda En La DB:
        public async Task<int> Registrar_Producto(Producto producto)
        {
            return await _ProductoDAL.Registrar_Producto(producto);
        }


        // Recibe Un Objeto Lo Busca Y Modifica El Encontrado Con El Nuevo:
        public async Task<int> Editar_Producto(Producto producto)
        {
            return await _ProductoDAL.Editar_Producto(producto);
        }


        // Recibe Un Objeto Lo Busca Y Elimina El Encontrado:
        public async Task<int> Eliminar_Producto(Producto producto)
        {
            return await _ProductoDAL.Eliminar_Producto(producto);
        }
    }
}
