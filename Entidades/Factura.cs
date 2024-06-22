using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entidades
{
    public class Factura
    {
        // ATRIBUTOS:
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFactura { get; set; }


        [Required(ErrorMessage ="Ingrese La Fecha En Que Se Realizo.")]
        public DateTime FechaRealizada { get; set; }


        [Required(ErrorMessage = "Ingrese Nombre Del Cliente.")]
        public string NombreCliente { get; set; }


        public int Correlativo { get; set; }


        public double Total { get; set; }


        [NotMapped]
        public DateTime? FechaBuscar { get; set; }

        [NotMapped]
        public string? MesBuscar { get; set; }



        // Tabla DetalleFcatura Asociada A Esta:
        public virtual List<DetalleFactura> Lista_DetalleFactura { get; set; }



        // Referencia Tabla Empleado:
        [Required(ErrorMessage = "Seleccione Al Empleado")]
        [ForeignKey("Objeto_Empleado")]
        public int IdEmpleadoEnFactura { get; set; }
        public virtual Empleado Objeto_Empleado { get; set; }


        // CONSTRUCTOR:
        public Factura()
        {
            Lista_DetalleFactura = new List<DetalleFactura>();
        }
    }
}
