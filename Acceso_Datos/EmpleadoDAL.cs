using Entidades;
using Microsoft.EntityFrameworkCore;


namespace Acceso_Datos
{
    public class EmpleadoDAL
    {
        // Representa La DB:
        private readonly MyDBcontext _MyDBcontext;

        // Constructor:
        public EmpleadoDAL(MyDBcontext myDBcontext)
        {
            _MyDBcontext = myDBcontext;
        }


        // ******* METODOS QUE MANDARAN OBJETOS A LAS VISTAS ********
        // **********************************************************

        // Manda Un Objeto Encontrado:
        public async Task<Empleado> Obtener_PorId(Empleado empleado)
        {
            Empleado? Objeto_Obtenido = await _MyDBcontext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == empleado.IdEmpleado);

            if(Objeto_Obtenido!=null)
            {
                return Objeto_Obtenido;
            }
            else
            {
                return new Empleado();
            }          
        }

        // Manda Todos Los Objetos:
        public async Task<List<Empleado>> Obtener_Todos()
        {
             List <Empleado> Objetos_Obtenidos = await _MyDBcontext.Empleados.ToListAsync();

            return Objetos_Obtenidos;
        }




        // *******  METODOS QUE RECIBIRAN OBJETOS Y MODIFICARAN LA DB  ********
        // ********************************************************************

        // Recibe Un Objeto Lo Guarda En La DB:
        public async Task<int> RegistrarEmpleado(Empleado empleado)
        {
            _MyDBcontext.Add(empleado);

            return await _MyDBcontext.SaveChangesAsync();
        }


        // Recibe Un Objeto Lo Busca Y Modifica El Encontrado Con El Nuevo:
        public async Task<int> EditarEmpleado(Empleado empleado)
        {
            Empleado? Objeto_Obtenido = await _MyDBcontext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == empleado.IdEmpleado);

            if (Objeto_Obtenido != null)
            {
                Objeto_Obtenido.Nombre = empleado.Nombre;
                Objeto_Obtenido.Salaraio = empleado.Salaraio;
                Objeto_Obtenido.Email = empleado.Email;
                Objeto_Obtenido.FechaNacimiento = empleado.FechaNacimiento;
                Objeto_Obtenido.Fotografia = empleado.Fotografia;

                _MyDBcontext.Update(Objeto_Obtenido);
            }

            return await _MyDBcontext.SaveChangesAsync();
        }

        // Recibe Un Objeto Lo Busca Y Elimina El Encontrado:
        public async Task<int> EliminarEmpleado(Empleado empleado)
        {
            Empleado? Objeto_Obtenido = await _MyDBcontext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == empleado.IdEmpleado);

            if (Objeto_Obtenido != null)
            {
                _MyDBcontext.Remove(Objeto_Obtenido);
            }

            return await _MyDBcontext.SaveChangesAsync();
        }

    }
}
