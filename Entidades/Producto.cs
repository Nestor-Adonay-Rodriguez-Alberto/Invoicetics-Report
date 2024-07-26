using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Entidades
{
    public class Producto
    {
        // ATRIBUTOS:
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }


        [Required(ErrorMessage = "Ingrese Un Nombre Al Producto.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Ingrese El Precio Del Producto.")]
        [Column(TypeName = "decimal(10,2)")]
        public double Precio { get; set; }


        // Tabla DetalleFcatura Asociada A Esta:
        public virtual List<DetalleFactura> Lista_DetalleFactura { get; set; }

        public Producto()
        {
            Lista_DetalleFactura = new List<DetalleFactura>();
        }
    }
}
