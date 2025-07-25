import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-modal-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './mdoal-form.component.html',
  styleUrl: './mdoal-form.component.css'
})
export class ModalFormComponent implements OnInit {
  @Input() title: string = '';
  @Input() fields: {
    key: string;
    label: string;
    type: string;
    options?: { label: string; value: any }[];
  }[] = [];
  @Input() model: any = {};
  @Output() save = new EventEmitter<any>();
  @Output() close = new EventEmitter<void>();

  form!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    const group: { [key: string]: any } = {};
  
    for (const field of this.fields) {
      const validators = [Validators.required];
  
      if (field.key === 'edad') {
        validators.push(Validators.min(18), Validators.max(99));
      }
  
      group[field.key] = [this.model[field.key] ?? null, validators];
    }
  
    this.form = this.fb.group(group);
  }

  onSubmit() {
    if (this.form.valid) {
      this.save.emit(this.form.value);
    }
  }

  onCancel() {
    this.close.emit();
  }
}

