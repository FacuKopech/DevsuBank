
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Movimiento
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public TipoMovimiento TipoMovimiento { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public Guid CuentaId { get; set; }
        public Cuenta Cuenta { get; set; } = null!;
    }

    public enum TipoMovimiento
    {
        Desposito, 
        Extraccion,
        TransferenciaEntrante,
        TransferenciaSaliente,
        DebitoAutomatico,
        PagoDebito,
        PagoCredito,
        CompraQR,
        PagoServicios
    }
}
