using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class MovimientoDto
    {
        public Guid Id { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public decimal Valor { get; set; }
        public Guid CuentaId { get; set; }
    }
}
