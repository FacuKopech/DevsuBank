import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Cliente } from '../../interfaces/cliente';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private readonly API_URL = 'https://localhost:7039/api/clientes';

  constructor(private http: HttpClient) { }

  GetAllClients(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(`${this.API_URL}/GetAllClients`);
  }

  CreateClient(client: Cliente): Observable<Cliente> {
    return this.http.post<Cliente>(`${this.API_URL}/CreateClient`, client);
  }

  UpdateClient(client: Cliente): Observable<boolean> {
    return this.http.patch<boolean>(`${this.API_URL}/UpdateClient`, client);
  }

  DeleteClient(personaId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.API_URL}/DeleteClient/${personaId}`);
  }
  
}
