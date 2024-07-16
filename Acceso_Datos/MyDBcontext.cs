using Entidades;
using Microsoft.EntityFrameworkCore;


namespace Acceso_Datos
{
    public class MyDBcontext: DbContext
    {
        // Constructor:
        public MyDBcontext(DbContextOptions<MyDBcontext> options)
            : base(options)
        {

        }

        // Tablas En La DB:
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public DbSet<Producto> Productos { get; set; }

    }
}
