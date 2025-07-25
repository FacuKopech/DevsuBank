import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Reporte } from '../../interfaces/reporte';

@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  private readonly API_URL = 'https://localhost:7039/api';

  constructor(private http: HttpClient) { }

  GenerateReport(reporte: Reporte): Observable<Reporte> {
    return this.http.get<Reporte>(`${this.API_URL}/Reportes?fechaInicio=${reporte.fechaInicio}&fechaFin=${reporte.fechaFin}&clienteId=${reporte.clienteId}`);
  }

}
