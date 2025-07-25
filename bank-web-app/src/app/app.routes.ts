import { Routes } from '@angular/router';
import { ClienteComponent } from './components/cliente/cliente.component';
import { CuentaComponent } from './components/cuenta/cuenta.component';
import { MovimientoComponent } from './components/movimiento/movimiento.component';
import { ReporteComponent } from './components/reporte/reporte.component';
import { WelcomeComponent } from './components/welcome/welcome.component';

export const routes: Routes = [
    { path: '', component: WelcomeComponent, pathMatch: 'full' },
    { path: 'clientes', component: ClienteComponent },
    { path: 'cuentas', component: CuentaComponent },
    { path: 'movimientos', component: MovimientoComponent },
    { path: 'reportes', component: ReporteComponent },
];
