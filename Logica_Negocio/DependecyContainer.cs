using Acceso_Datos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Logica_Negocio
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddBLDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDALDependecies(configuration);

            // Registrando Las Clases:
            services.AddScoped<EmpleadoBL>();
            services.AddScoped<FacturaBL>();
            services.AddScoped<ReportesBL>();
            services.AddScoped<ProductoBL>();


            return services;
        }
    }
}
