using Entidades;
using Microsoft.EntityFrameworkCore;


namespace Acceso_Datos
{
    public class FacturaDAL
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;

        // Constructor:
        public FacturaDAL(MyDBcontext myDBcontext)
        {
            _MyDBcontext = myDBcontext;
        }


        // ******* METODOS QUE MANDARAN OBJETOS A LAS VISTAS ********
        // **********************************************************

        // Manda Un Objeto Encontrado:
        public async Task<Factura> Obtener_PorId(Factura factura)
        {
            Factura? Objeto_Obtenido = await _MyDBcontext.Facturas
                .Include(x => x.Objeto_Empleado)
                .Include(x => x.Lista_DetalleFactura)
                .FirstOrDefaultAsync(x => x.IdFactura == factura.IdFactura);

            if (Objeto_Obtenido != null)
            {
                return Objeto_Obtenido;
            }
            else
            {
                return new Factura();
            }
        }


        // Manda Todos Los Objetos:
        public async Task<List<Factura>> Obtener_Todas()
        {
            List<Factura> Objetos_Obtenidos = await _MyDBcontext.Facturas
                .Include(x => x.Objeto_Empleado)
                .ToListAsync();

            return Objetos_Obtenidos;
        }

        // Lista De La Tabla Empleado Para ViewData:
        public async Task<List<Empleado>> Lista_Empleados()
        {
            List<Empleado> Objetos_Obtenidos = await _MyDBcontext.Empleados.ToListAsync();

            return Objetos_Obtenidos;
        }




        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  ********
        // ********************************************************************

        // Recibe Un Objeto Lo Guarda En La DB:
        public async Task<int> Registrar_Factura(Factura factura)
        {
            _MyDBcontext.Add(factura);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // Recibe Un Objeto Lo Busca Y Modifica El Encontrado Con El Nuevo:
        public async Task<int> Editar_Factura(Factura factura)
        {
            Factura? Objeto_Obtenido = await _MyDBcontext.Facturas
                .Include(x => x.Objeto_Empleado)
                .Include(x => x.Lista_DetalleFactura)
                .FirstOrDefaultAsync(x => x.IdFactura == factura.IdFactura);

            if (Objeto_Obtenido != null)
            {
                // NUEVOS DATOS DE FACTURA:
                Objeto_Obtenido.FechaRealizada = factura.FechaRealizada;
                Objeto_Obtenido.NombreCliente = factura.NombreCliente;
                Objeto_Obtenido.IdEmpleadoEnFactura = factura.IdEmpleadoEnFactura;
                Objeto_Obtenido.Total=factura.Total;

                // NUEVOS DETALLES AGREGARLOS A LAS LISTA:
                Agregar_Detalles(Objeto_Obtenido, factura);


                // DETALLES EXISTENTES DE LA LISTA "Podrian Traer Cambios":
                Editar_Detalles(Objeto_Obtenido, factura);


                // ELIMINAR LOS DETALLES DE LA LISTA:
                Eliminar_Detalles(Objeto_Obtenido, factura);


                _MyDBcontext.Update(Objeto_Obtenido);
            }

            //Recalcular 
            Objeto_Obtenido.Total = Objeto_Obtenido.Lista_DetalleFactura.Sum(s => s.CantidadComprada * s.PrecioProducto);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // METODOS PARA MODIFICAR EL DETALLE DE LA FACTURA
        // *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** ***

        // NUEVOS DETALLES AGREGARLOS A LAS LISTA:
        private void Agregar_Detalles(Factura Objeto_Obtenido, Factura factura)
        {
            IEnumerable<DetalleFactura> Detalles_Nuevos = factura.Lista_DetalleFactura.Where(s => s.IdDetalleFactura == 0);
            foreach (DetalleFactura Detalle_Nuevo in Detalles_Nuevos)
            {
                Objeto_Obtenido.Lista_DetalleFactura.Add(Detalle_Nuevo);
            }
        }


        // DETALLES EXISTENTES DE LA LISTA "Podrian Traer Cambios":
        private void Editar_Detalles(Factura Objeto_Obtenido, Factura factura)
        {
            IEnumerable<DetalleFactura> Detalles_Lista = factura.Lista_DetalleFactura.Where(s => s.IdDetalleFactura > 0);
            foreach (DetalleFactura Detalle in Detalles_Lista)
            {
                // Detalle En La Lista De La Factura Encontrada
                DetalleFactura? Detalle_EnLista = Objeto_Obtenido.Lista_DetalleFactura.FirstOrDefault(s => s.IdDetalleFactura == Detalle.IdDetalleFactura);

                Detalle_EnLista.NombreProducto = Detalle.NombreProducto;
                Detalle_EnLista.CantidadComprada = Detalle.CantidadComprada;
                Detalle_EnLista.PrecioProducto = Detalle.PrecioProducto;

            }
        }


        // ELIMINAR LOS DETALLES DE LA LISTA:
        private void Eliminar_Detalles(Factura Objeto_Obtenido, Factura factura)
        {
            IEnumerable<DetalleFactura> Detalles_Eliminar = factura.Lista_DetalleFactura.Where(s => s.IdDetalleFactura < 0).ToList();

            if (Detalles_Eliminar != null)
            {
                foreach (DetalleFactura Detalle_Eliminar in Detalles_Eliminar)
                {
                    Detalle_Eliminar.IdDetalleFactura = Detalle_Eliminar.IdDetalleFactura * -1;

                    DetalleFactura? Detalle_ListaEliminar = Objeto_Obtenido.Lista_DetalleFactura.FirstOrDefault(s => s.IdDetalleFactura == Detalle_Eliminar.IdDetalleFactura);

                    _MyDBcontext.Remove(Detalle_ListaEliminar);
                    _MyDBcontext.SaveChanges();
                }
            }
        }

        // *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** *** ***


        // Recibe Un Objeto Lo Busca Y Elimina El Encontrado:
        public async Task<int> Eliminar_Factura(Factura factura)
        {
            Factura? Objeto_Obtenido = await _MyDBcontext.Facturas.FirstOrDefaultAsync(m => m.IdFactura == factura.IdFactura);

            if (Objeto_Obtenido != null)
            {
                _MyDBcontext.Remove(Objeto_Obtenido);
            }

            return await _MyDBcontext.SaveChangesAsync();
        }
    }
}
