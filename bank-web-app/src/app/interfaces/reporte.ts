import { Resumen } from "./resumen";

export interface Reporte {
    clienteId: string;
    fechaInicio: Date;
    fechaFin: Date;
    resumenes: Resumen[];
}
