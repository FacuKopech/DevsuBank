import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal-message',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal-message.component.html',
  styleUrl: './modal-message.component.css'
})
export class ModalMessageComponent {
  @Input() title: string = '';
  @Input() message: string = '';
  @Input() isError: boolean = false;
  @Output() close = new EventEmitter<void>();

  closeModal() {
    this.close.emit();
  }
}
