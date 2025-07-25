import { Component, OnInit } from '@angular/core';
import { TableComponent } from '../shared/table/table.component';
import { ClienteService } from '../../services/cliente-service/cliente.service';
import { Cliente } from '../../interfaces/cliente';
import { ModalFormComponent } from '../shared/mdoal-form/mdoal-form.component';
import { CommonModule } from '@angular/common';
import { MdoalConfirmationComponent } from '../shared/mdoal-confirmation/mdoal-confirmation.component';
import { ModalMessageComponent } from '../shared/modal-message/modal-message.component';

@Component({
  selector: 'app-cliente',
  standalone: true,
  imports: [TableComponent, ModalFormComponent, CommonModule, MdoalConfirmationComponent, ModalMessageComponent],
  templateUrl: './cliente.component.html',
  styleUrl: './cliente.component.css'
})
export class ClienteComponent implements OnInit {
  constructor(private clientService: ClienteService) { }
  showModal = false;
  showConfirmModal = false;
  cliente: Cliente = {} as Cliente;
  clientes: Cliente[] = [];
  modalTitle: string = '';
  message: string = '';
  messageTitle: string = '';
  showMessageModal: boolean = false;
  isError: boolean = false;

  formFields = [
    { key: 'nombre', label: 'Nombre', type: 'text' },
    {
      key: 'genero', label: 'Género', type: 'select', options: [
        { label: 'Masculino', value: 'Masculino' },
        { label: 'Femenino', value: 'Femenino' }
      ]
    },
    { key: 'edad', label: 'Edad', type: 'number' },
    { key: 'identificacion', label: 'Identificación', type: 'text' },
    { key: 'direccion', label: 'Dirección', type: 'text' },
    { key: 'telefono', label: 'Teléfono', type: 'number' },
    { key: 'contraseña', label: 'Contraseña', type: 'password' },
    {
      key: 'estado', label: 'Estado', type: 'number', options: [
        { label: 'Activo', value: 0 },
        { label: 'Inactivo', value: 1 }
      ]
    }
  ];

  handleResponse(result: { success: boolean, message: string }) {
    this.messageTitle = result.success ? 'Excelente' : 'Error';
    this.message = result.message;
    this.isError = !result.success;
    this.showMessageModal = true;
  }

  ngOnInit(): void {
    this.clientService.GetAllClients().subscribe(data => {
      this.clientes = data;
    });
  }

  openModal() {
    this.cliente = {} as Cliente;
    this.modalTitle = 'Crear Cliente';
    this.formFields = [...this.formFields];
    this.showModal = true;
  }

  openEditModal(cliente: Cliente) {
    this.cliente = { ...cliente };
    this.modalTitle = 'Modificar Cliente';
    this.formFields = this.formFields.filter(f => f.key !== 'contraseña');
    this.showModal = true;
  }

  openDeleteModal(client: Cliente) {
    this.cliente = client;
    this.showConfirmModal = true;
  }

  confirmDelete() {
    if (!this.cliente) return;
    this.clientService.DeleteClient(this.cliente.id).subscribe({
      next: () => {
        this.clientes = this.clientes.filter(c => c.id !== this.cliente?.id);
        this.showConfirmModal = false;
        this.handleResponse({ success: true, message: 'Cliente eliminado correctamente' });
      },
      error: err => {
        this.handleResponse({ success: false, message: err?.error || 'Error al eliminar el cliente' });
      }
    });
  }

  cancelDelete() {
    this.showConfirmModal = false;
  }

  handleClientSave(client: Cliente) {
    const clientReceived = { ...this.cliente, ...client };
    if (clientReceived.id == undefined) {
      console.log(this.cliente)
      this.clientService.CreateClient(clientReceived).subscribe({
        next: data => {
          this.clientes.push(data);
          this.showModal = false;
          this.handleResponse({ success: true, message: 'Cliente creado correctamente' });
        },
        error: err => {
          this.handleResponse({ success: false, message: err?.error || 'Error al crear el cliente' });
        }
      });
    } else {
      this.clientService.UpdateClient(clientReceived).subscribe({
        next: () => {
          const index = this.clientes.findIndex(c => c.id === clientReceived.id);
          if (index !== -1) {
            this.clientes[index] = clientReceived;
          }
          this.showModal = false;
          this.handleResponse({ success: true, message: 'Cliente modificado correctamente' });
        },
        error: err => {
          this.handleResponse({ success: false, message: err?.error || 'Error al modificar el cliente' });
        }
      });
    }
  }
}