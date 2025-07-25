import { Cuenta } from "./cuenta";

export interface Movimiento {
    id: string;
    fecha: Date;
    tipoMovimiento: number;
    valor: number;
    saldo: number;
    cuentaId: string
    cuenta: Cuenta;
}
