using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace Acceso_Datos
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddDALDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            // Inyeccion
            services.AddDbContext<MyDBcontext>(options => options.UseSqlServer(configuration.GetConnectionString("Cadena_Conexion")));

            // Registrando Las Clases:
            services.AddScoped<EmpleadoDAL>();
            services.AddScoped<FacturaDAL>();
            services.AddScoped<ReportesDAL>();
            services.AddScoped<ProductoDAL>();

            return services;
        }
    }
}
