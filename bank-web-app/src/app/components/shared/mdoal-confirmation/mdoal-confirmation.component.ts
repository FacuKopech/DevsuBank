import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-mdoal-confirmation',
  standalone: true,
  imports: [],
  templateUrl: './mdoal-confirmation.component.html',
  styleUrl: './mdoal-confirmation.component.css'
})
export class MdoalConfirmationComponent {
  @Input() title: string = '';
  @Input() text: string = '';
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onConfirm() {
    this.confirm.emit();
  }

  onCancel() {
    this.cancel.emit();
  }
}
