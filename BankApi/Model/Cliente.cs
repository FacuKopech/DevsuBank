using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Cliente : Persona
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Contraseña { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public EstadoCliente Estado { get; set; }
        public ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
    }

    public enum EstadoCliente
    {
        Activo = 0,
        Inactivo = 1
    }
}
