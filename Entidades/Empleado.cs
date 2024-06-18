using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entidades
{
    public class Empleado
    {
        // ATRIBUTOS:
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEmpleado { get; set; }
        

        [Required(ErrorMessage ="Ingrese Un Nombre.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage ="Ingrese Un Salario.")]
        public double Salaraio { get; set; }


        [Required(ErrorMessage = "Ingrese Una Fecha De Nacimiento.")]
        public DateTime FechaNacimiento { get; set; }


        [Required(ErrorMessage = "Ingrese Un Correo Electronico.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lo Ingresado No Es Un Email.")]

        public string Email { get; set; }


        public byte[]? Fotografia { get; set; }


        // Tabla Factura Asociada A Esta:
        public virtual List<Factura> Lista_Facturas { get; set; }


        // CONSTRUCTOR:
        public Empleado()
        {
            Lista_Facturas = new List<Factura>();
        }
    }
}
