using Model;

namespace DTOs
{
    public class ClienteDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int Telefono { get; set; }
        public EstadoCliente Estado { get; set; }
        public ICollection<CuentaDto> Cuentas { get; set; } = new List<CuentaDto>();
    }
}