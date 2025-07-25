using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Resumen
    {
        public int NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }
        public int TotalDebitos { get; set; }
        public int TotalCreditos { get; set; }
    }
}
