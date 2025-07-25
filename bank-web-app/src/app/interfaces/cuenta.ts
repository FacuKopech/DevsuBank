import { Cliente } from "./cliente";
import { Movimiento } from "./movimiento";

export interface Cuenta {
    id: string;
    numeroCuenta: number;
    tipoCuenta: number;
    saldoInicial: number;
    estado: boolean;
    clienteId: string;
    cliente: Cliente;
    movimientos: Movimiento[];
}
