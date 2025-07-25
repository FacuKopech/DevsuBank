import { Cuenta } from "./cuenta";

interface Persona{
    id: string;
    nombre: string;
    genero: string;
    edad: number
    identificacion: string;
    direccion: string;
    telefono: number;
}

export interface Cliente extends Persona {
    contraseña: string;
    estado: number; 
    cuentas: Cuenta[]
}
