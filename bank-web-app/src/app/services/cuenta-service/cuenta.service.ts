import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Cuenta } from '../../interfaces/cuenta';

@Injectable({
  providedIn: 'root'
})
export class CuentaService {

  private readonly API_URL = 'https://localhost:7039/api/cuentas';

  constructor(private http: HttpClient) { }

  GetAllAccounts(): Observable<Cuenta[]> {
    return this.http.get<Cuenta[]>(`${this.API_URL}/GetAllAccounts`);
  }

  CreateAccount(account: Cuenta): Observable<Cuenta> {
    return this.http.post<Cuenta>(`${this.API_URL}/CreateAccount`, account);
  }

  UpdateAccount(account: Cuenta): Observable<boolean> {
    return this.http.patch<boolean>(`${this.API_URL}/UpdateAccount`, account);
  }

  DeleteAccount(accountId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.API_URL}/DeleteAccount/${accountId}`);
  }
}
