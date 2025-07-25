import { Component, OnInit, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { Cliente } from '../../interfaces/cliente';
import { ClienteService } from '../../services/cliente-service/cliente.service';
import { ReporteService } from '../../services/reporte-service/reporte.service';
import { Reporte } from '../../interfaces/reporte';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import jsPDF from 'jspdf';
import domtoimage from 'dom-to-image';
import { ModalMessageComponent } from '../shared/modal-message/modal-message.component';

@Component({
  selector: 'app-reporte',
  standalone: true,
  imports: [CommonModule, FormsModule, ModalMessageComponent],
  templateUrl: './reporte.component.html',
  styleUrl: './reporte.component.css'
})
export class ReporteComponent implements OnInit {
  clients: Cliente[] = [];
  reportRequest: Reporte = {
    clienteId: '',
    fechaInicio: new Date(),
    fechaFin: new Date(),
    resumenes: [],
  };
  report?: Reporte;
  nombreCliente?: string = '';
  message: string = '';
  messageTitle: string = '';
  showMessageModal: boolean = false;
  isError: boolean = false;
  @ViewChild('reportTable') reportTable!: ElementRef;

  constructor(
    private clienteService: ClienteService,
    private reporteService: ReporteService
  ) { }

  handleResponse(result: { success: boolean, message: string }) {
    this.messageTitle = result.success ? 'Excelente' : 'Error';
    this.message = result.message;
    this.isError = !result.success;
    this.showMessageModal = true;
  }

  ngOnInit(): void {
    this.clienteService.GetAllClients().subscribe((data) => {
      this.clients = data;
    });
  }

  generateReport() {
    console.log('Request being sent:', this.reportRequest);
    this.reporteService.GenerateReport(this.reportRequest).subscribe({
      next: (report: Reporte) => {
        this.report = report;
        this.getNombreCliente(this.reportRequest.clienteId);
        this.handleResponse({ success: true, message: 'Reporte generado correctamente' });
      },
      error: err => {
        this.handleResponse({ success: false, message: err?.error || 'Error al generar reporte' });
      }
    });
  }

  getNombreCliente(clienteId: string) {
    this.nombreCliente = this.clients.find(client => client.id == clienteId)?.nombre
  }

  exportToPDF() {
    const node = this.reportTable?.nativeElement as HTMLElement;

    const options = {
      background: 'white',
      style: {
        transform: 'scale(1)',
        transformOrigin: 'top left',
      }
    };

    domtoimage.toPng(node, options)
      .then((dataUrl: string) => {
        const pdf = new jsPDF('p', 'mm', 'a4');
        const img = new Image();
        img.src = dataUrl;

        img.onload = () => {
          const pdfWidth = pdf.internal.pageSize.getWidth();
          const ratio = img.height / img.width;
          const pdfHeight = pdfWidth * ratio;

          pdf.addImage(img, 'PNG', 0, 10, pdfWidth, pdfHeight);
          pdf.save('reporte.pdf');
        };
      })
      .catch(error => {
        console.error('Failed to export PDF', error);
      });
  }
}
