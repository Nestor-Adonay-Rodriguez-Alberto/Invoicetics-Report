using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;



namespace Acceso_Datos
{
    public class MyDBContextFactory : IDesignTimeDbContextFactory<MyDBcontext>
    {
        public MyDBcontext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDBcontext>();
            const string Cadena_Conexion = "Data Source=.;Initial Catalog=Z3_Invoicetics_Report;Integrated Security=True;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(Cadena_Conexion);

            return new MyDBcontext(optionsBuilder.Options);
        }
    }
}
