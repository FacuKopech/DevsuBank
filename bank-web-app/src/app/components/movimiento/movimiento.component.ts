import { Component } from '@angular/core';
import { TableComponent } from '../shared/table/table.component';
import { ModalFormComponent } from '../shared/mdoal-form/mdoal-form.component';
import { CommonModule } from '@angular/common';
import { MdoalConfirmationComponent } from '../shared/mdoal-confirmation/mdoal-confirmation.component';
import { MovimientoService } from '../../services/movimiento-service/movimiento.service';
import { CuentaService } from '../../services/cuenta-service/cuenta.service';
import { Movimiento } from '../../interfaces/movimiento';
import { Cuenta } from '../../interfaces/cuenta';
import { concatMap } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { ModalMessageComponent } from '../shared/modal-message/modal-message.component';

@Component({
  selector: 'app-movimiento',
  standalone: true,
  imports: [TableComponent, ModalFormComponent, CommonModule, MdoalConfirmationComponent, FormsModule, ModalMessageComponent],
  templateUrl: './movimiento.component.html',
  styleUrl: './movimiento.component.css'
})
export class MovimientoComponent {
  constructor(private movimientoService: MovimientoService, private cuentaService: CuentaService) { }
  showModal = false;
  showConfirmModal = false;
  movimiento: Movimiento = {} as Movimiento;
  movimientos: Movimiento[] = [];
  cuentas: Cuenta[] = [];
  modalTitle: string = '';
  fechaInicio: Date = new Date();
  fechaFin: Date = new Date();
  message: string = '';
  messageTitle: string = '';
  showMessageModal: boolean = false;
  isError: boolean = false;

  formFields = [
    {
      key: 'tipoMovimiento', label: 'Tipo Movimiento', type: 'number', options: [
        { label: 'Desposito', value: 0 },
        { label: 'Extraccion', value: 1 },
        { label: 'Transferencia Entrante', value: 2 },
        { label: 'Transferencia Saliente', value: 3 },
        { label: 'Debito Automatico', value: 4 },
        { label: 'Pago Debito', value: 5 },
        { label: 'Pago Credito', value: 6 },
        { label: 'Compra QR', value: 7 },
        { label: 'Pago Servicios', value: 8 },
      ]
    },
    { key: 'valor', label: 'Valor', type: 'number' },
    {
      key: 'cuentaId', label: 'Cuenta', type: 'select', options: [] as { label: string; value: string }[]
    }
  ];

  handleResponse(result: { success: boolean, message: string }) {
    this.messageTitle = result.success ? 'Excelente' : 'Error';
    this.message = result.message;
    this.isError = !result.success;
    this.showMessageModal = true;
  }

  ngOnInit(): void {
    this.movimientoService.GetAllTransactions().pipe(
      concatMap(movimientos => {
        this.movimientos = movimientos;
        return this.cuentaService.GetAllAccounts();
      })
    ).subscribe(cuentas => {
      this.cuentas = cuentas;
      this.cuentas = this.cuentas.filter(c => c.estado === true);
      this.formFields = this.formFields.map(field => {
        if (field.key === 'cuentaId') {
          console.log(this.cuentas);
          return {
            ...field,
            options: this.cuentas.map(c => ({
              label: `Cuenta Nro. ${c.numeroCuenta} - ${c.tipoCuenta === 0 ? 'Ahorro' :
                c.tipoCuenta === 1 ? 'Corriente' :
                  c.tipoCuenta === 2 ? 'Sueldo' :
                    'Desconocido'
                } - ${c.cliente.nombre}`,
              value: c.id
            }))
          };
        }
        return field;
      });
    });
  }

  openModal() {
    this.movimiento = {} as Movimiento;
    this.modalTitle = 'Crear Movimiento';
    this.showModal = true;
  }

  openEditModal(movimiento: Movimiento) {
    this.movimiento = { ...movimiento };
    this.modalTitle = 'Modificar Movimiento';
    this.showModal = true;
  }

  openDeleteModal(movimiento: Movimiento) {
    this.movimiento = movimiento;
    this.showConfirmModal = true;
  }

  confirmDelete() {
    if (!this.movimiento) return;
    this.movimientoService.DeleteTransaction(this.movimiento.id).subscribe({
      next: () => {
        this.movimientos = this.movimientos.filter(c => c.id !== this.movimiento?.id);
        this.showConfirmModal = false;
        this.handleResponse({ success: true, message: 'Movimiento eliminado correctamente' });
      },
      error: err => {
        this.handleResponse({ success: false, message: err?.error || 'Error eliminando el movimiento' });
      }
    });
  }

  cancelDelete() {
    this.showConfirmModal = false;
  }

  handleClientSave(movimiento: Movimiento) {
    const transactionReceived = { ...this.movimiento, ...movimiento };
    if (transactionReceived.id == undefined) {
      this.movimientoService.CreateTransaction(transactionReceived).subscribe({
        next: data => {
          this.movimientos.push(data);
          this.showModal = false;
          this.handleResponse({ success: true, message: 'Movimiento creado correctamente' });
        },
        error: err => {
          this.handleResponse({ success: false, message: err?.error || 'Error creando el movimiento' });
        }
      });
    } else {
      this.movimientoService.UpdateTransaction(transactionReceived).subscribe({
        next: () => {
          const index = this.movimientos.findIndex(c => c.id === transactionReceived.id);
          if (index !== -1) {
            this.movimientos[index] = transactionReceived;
          }
          this.showModal = false;
          this.handleResponse({ success: true, message: 'Movimiento actualizado correctamente' });
        },
        error: err => {
          this.handleResponse({ success: false, message: err?.error || 'Error actualizando el movimiento' });
        }
      });
    }
  }
}
