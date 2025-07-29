import { Component } from '@angular/core';
import { CuentaService } from '../../services/cuenta-service/cuenta.service';
import { Cuenta } from '../../interfaces/cuenta';
import { concatMap } from 'rxjs';
import { ClienteService } from '../../services/cliente-service/cliente.service';
import { Cliente } from '../../interfaces/cliente';
import { ModalFormComponent } from '../shared/mdoal-form/mdoal-form.component';
import { TableComponent } from '../shared/table/table.component';
import { CommonModule } from '@angular/common';
import { MdoalConfirmationComponent } from '../shared/mdoal-confirmation/mdoal-confirmation.component';
import { ModalMessageComponent } from '../shared/modal-message/modal-message.component';
import { ResponseHandlerService } from '../../services/response-handler-service/response-handler.service';

@Component({
  selector: 'app-cuenta',
  standalone: true,
  imports: [TableComponent, ModalFormComponent, CommonModule, MdoalConfirmationComponent, ModalMessageComponent],
  templateUrl: './cuenta.component.html',
  styleUrl: './cuenta.component.css'
})
export class CuentaComponent {
  constructor(private cuentaService: CuentaService, private clienteService: ClienteService, public responseHandler: ResponseHandlerService) { }
  showModal = false;
  showConfirmModal = false;
  cuenta: Cuenta = {} as Cuenta;
  cuentas: Cuenta[] = [];
  clientes: Cliente[] = [];
  modalTitle: string = '';

  formFields = [
    { key: 'numeroCuenta', label: 'Nro. Cuenta', type: 'number' },
    {
      key: 'tipoCuenta', label: 'Tipo de Cuenta', type: 'select', options: [
        { label: 'Ahorro', value: 0 },
        { label: 'Corriente', value: 1 },
        { label: 'Sueldo', value: 2 }
      ]
    },
    { key: 'saldoInicial', label: 'Saldo Inicial', type: 'number' },
    {
      key: 'estado', label: 'Estado', type: 'boolean', options: [
        { label: 'Activo', value: true },
        { label: 'Inactivo', value: false }
      ]
    },
    {
      key: 'clienteId', label: 'Cliente', type: 'select', options: [] as { label: string; value: string }[]
    }
  ];

  ngOnInit(): void {
    this.cuentaService.GetAllAccounts().pipe(
      concatMap(cuentas => {
        this.cuentas = cuentas;
        return this.clienteService.GetAllClients();
      })
    ).subscribe(clientes => {
      this.clientes = clientes;
      this.clientes = this.clientes.filter(c => c.estado === 0);
      this.formFields = this.formFields.map(field => {
        if (field.key === 'clienteId') {
          return {
            ...field,
            options: this.clientes.map(c => ({
              label: c.nombre,
              value: c.id
            }))
          };
        }
        return field;
      });
    });
  }

  openModal() {
    this.cuenta = {} as Cuenta;
    this.modalTitle = 'Crear Cuenta';
    this.showModal = true;
  }

  openEditModal(cuenta: Cuenta) {
    this.cuenta = { ...cuenta };
    this.modalTitle = 'Modificar Cuenta';
    this.showModal = true;
  }

  openDeleteModal(cuenta: Cuenta) {
    this.cuenta = cuenta;
    this.showConfirmModal = true;
  }

  confirmDelete() {
    if (!this.cuenta) return;
    this.cuentaService.DeleteAccount(this.cuenta.id).subscribe({
      next: () => {
        this.cuentas = this.cuentas.filter(c => c.id !== this.cuenta?.id);
        this.showConfirmModal = false;
        this.responseHandler.handleResponse({ success: true, message: 'Cuenta eliminada correctamente' });
      },
      error: err => {
        this.responseHandler.handleResponse({ success: false, message: err?.error || 'Error al eliminar la cuenta' });
      }
    });
  }

  cancelDelete() {
    this.showConfirmModal = false;
  }

  handleAccountSave(cuenta: Cuenta) {
    const accountReceived = { ...this.cuenta, ...cuenta };
    if (accountReceived.id == undefined) {
      this.cuentaService.CreateAccount(accountReceived).subscribe({
        next: data => {
          this.cuentas.push(data);
          this.showModal = false;
          this.responseHandler.handleResponse({ success: true, message: 'Cuenta creada correctamente' });
        },
        error: err => {
          this.responseHandler.handleResponse({ success: false, message: err?.error || 'Error al crear la cuenta' });
        }
      });
    } else {
      this.cuentaService.UpdateAccount(accountReceived).subscribe({
        next: () => {
          const index = this.cuentas.findIndex(c => c.id === accountReceived.id);
          if (index !== -1) {
            this.cuentas[index] = accountReceived;
          }
          this.showModal = false;
          this.responseHandler.handleResponse({ success: true, message: 'Cuenta modificada correctamente' });
        },
        error: err => {
          this.responseHandler.handleResponse({ success: false, message: err?.error || 'Error al modificar la cuenta' });
        }
      });
    }
  }
}
