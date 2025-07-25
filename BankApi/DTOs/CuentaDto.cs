using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class CuentaDto
    {
        public Guid Id { get; set; }
        public int NumeroCuenta { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        public Guid ClienteId { get; set; }
    }
}
