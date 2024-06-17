using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entidades
{
    public class DetalleFactura
    {
        // ATRIBUTOS:
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetalleFactura { get; set; }

        [Required(ErrorMessage ="Ingrese El Nombre Del Producto A LLevar.")]
        public string NombreProducto { get; set; }


        [Required(ErrorMessage = "Ingrese La Cantidad A LLevar.")]
        public int CantidadComprada { get; set; }


        [Required(ErrorMessage = "Ingrese El Precio Del Producto.")]
        public double PrecioProducto { get; set; }


        // Referencia Tabla Empleado:
        [ForeignKey("Objeto_Factura")]
        public int IdFacturaEnDetalleFactura { get; set; }
        public virtual Factura Objeto_Factura { get; set; }
    }
}
