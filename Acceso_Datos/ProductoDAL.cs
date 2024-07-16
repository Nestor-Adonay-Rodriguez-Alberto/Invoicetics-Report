using Entidades;
using Microsoft.EntityFrameworkCore;


namespace Acceso_Datos
{
    public class ProductoDAL
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;

        // Constructor:
        public ProductoDAL(MyDBcontext myDBcontext)
        {
            _MyDBcontext = myDBcontext;
        }


        // ******* METODOS QUE MANDARAN OBJETOS A LAS VISTAS ********
        // **********************************************************

        // Manda Un Objeto Encontrado:
        public async Task<Producto> Obtener_PorId(Producto producto)
        {
            Producto? Objeto_Obtenido = await _MyDBcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == producto.IdProducto);

            if (Objeto_Obtenido != null)
            {
                return Objeto_Obtenido;
            }
            else
            {
                return new Producto();
            }
        }


        // Manda Todos Los Objetos:
        public async Task<List<Producto>> Obtener_Todos()
        {
            List<Producto> Objetos_Obtenidos = await _MyDBcontext.Productos.ToListAsync();

            return Objetos_Obtenidos;
        }



        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  ********
        // ********************************************************************

        // Recibe Un Objeto Lo Guarda En La DB:
        public async Task<int> Registrar_Producto(Producto producto)
        {
            _MyDBcontext.Productos.Add(producto);

            return await _MyDBcontext.SaveChangesAsync();
        }

        // Recibe Un Objeto Lo Busca Y Modifica El Encontrado Con El Nuevo:
        public async Task<int> Editar_Producto(Producto producto)
        {
            Producto? Objeto_Obtenido = await _MyDBcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == producto.IdProducto);

            if (Objeto_Obtenido != null)
            {
                Objeto_Obtenido.Nombre = producto.Nombre;
                Objeto_Obtenido.Precio = producto.Precio;

                _MyDBcontext.Productos.Update(Objeto_Obtenido);
            }

            return await _MyDBcontext.SaveChangesAsync();
        }

        // Recibe Un Objeto Lo Busca Y Elimina El Encontrado:
        public async Task<int> Eliminar_Producto(Producto producto)
        {
            Producto? Objeto_Obtenido = await _MyDBcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == producto.IdProducto);

            if (Objeto_Obtenido != null)
            {
                _MyDBcontext.Productos.Remove(Objeto_Obtenido);
            }

            return await _MyDBcontext.SaveChangesAsync();

        }
    }
}
