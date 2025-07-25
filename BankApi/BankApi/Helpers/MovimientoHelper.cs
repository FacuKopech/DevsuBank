using Model;

namespace BankApi.Helpers
{
    public class MovimientoHelper
    {
        public static bool IsDebit(TipoMovimiento tipoMovimiento)
        {
            return tipoMovimiento switch
            {
                TipoMovimiento.Extraccion
                or TipoMovimiento.TransferenciaSaliente
                or TipoMovimiento.DebitoAutomatico
                or TipoMovimiento.PagoDebito
                or TipoMovimiento.PagoCredito
                or TipoMovimiento.CompraQR
                or TipoMovimiento.PagoServicios => true,
                _ => false
            };
        }
    }
}
