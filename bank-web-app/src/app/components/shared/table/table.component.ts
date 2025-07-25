import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class TableComponent {
  @Input() data: any[] = [];
  @Input() columns: string[] = [];
  @Input() headers: string[] = [];
  @Input() searchTerm: string = '';
  @Input() filters: { clienteId?: string; fechaInicio?: Date; fechaFin?: Date } | null = null;
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();
  searchTermTransactions: string = '';

  get filteredTransactions(): any[] {
    let filtered = this.data;
  
    if (this.filters) {
      if (this.filters.clienteId) {
        filtered = filtered.filter(d => d.cuenta?.clienteId === this.filters!.clienteId);
      }
      if (this.filters.fechaInicio) {
        filtered = filtered.filter(d => new Date(d.fecha) >= new Date(this.filters!.fechaInicio!));
      }
      if (this.filters.fechaFin) {
        filtered = filtered.filter(d => new Date(d.fecha) <= new Date(this.filters!.fechaFin!));
      }
    }
  
    if (this.searchTermTransactions) {
      filtered = filtered.filter(item =>
        JSON.stringify(item).toLowerCase().includes(this.searchTermTransactions.toLowerCase())
      );
    }
  
    return filtered;
  }
  
  get filteredData() {
    if (!this.searchTerm) return this.data;
    const lower = this.searchTerm.toLowerCase();
    return this.data.filter(row =>
      Object.values(row).some(val =>
        String(val).toLowerCase().includes(lower)
      )
    );
  }

  getEstadoLabel(value: boolean | number): string {
    return typeof value === 'boolean'
      ? value ? 'Activo' : 'Inactivo'
      : value === 0 ? 'Activo' : 'Inactivo';
  }  

  getValue(item: any, path: string): any {
    return path.split('.').reduce((acc, part) => acc?.[part], item);
  }  
}
