import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Movimiento } from '../../interfaces/movimiento';

@Injectable({
  providedIn: 'root'
})
export class MovimientoService {
  private readonly API_URL = 'https://localhost:7039/api/movimientos';

  constructor(private http: HttpClient) { }

  GetAllTransactions(): Observable<Movimiento[]> {
    return this.http.get<Movimiento[]>(`${this.API_URL}/GetAllTransactions`);
  }

  CreateTransaction(transaction: Movimiento): Observable<Movimiento> {
    return this.http.post<Movimiento>(`${this.API_URL}/CreateTransaction`, transaction);
  }

  UpdateTransaction(transaction: Movimiento): Observable<boolean> {
    return this.http.patch<boolean>(`${this.API_URL}/UpdateTransaction`, transaction);
  }

  DeleteTransaction(transactionId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.API_URL}/DeleteTransaction/${transactionId}`);
  }
}
