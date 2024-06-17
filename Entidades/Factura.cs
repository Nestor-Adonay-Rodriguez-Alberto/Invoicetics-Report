using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        // Tabla DetalleFcatura Asociada A Esta:
        public virtual List<DetalleFactura> Lista_DetalleFactura { get; set; }



        // Referencia Tabla Empleado:
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
