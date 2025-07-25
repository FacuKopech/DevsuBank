
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Model
{
    public class Cuenta
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int NumeroCuenta { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public TipoCuenta TipoCuenta { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public decimal SaldoInicial { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public Cliente Cliente { get; set; } = null!;
        public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
    public enum TipoCuenta
    {
        Ahorro,
        Corriente,
        Sueldo,
    }
}
