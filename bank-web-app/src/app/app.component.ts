import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { SpinnerService } from './services/spinner.service';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, RouterOutlet, SpinnerComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  isLoading = false;
  constructor(private spinnerService: SpinnerService) {
    this.spinnerService.loading$.subscribe(value => {
      this.isLoading = value;
    });
    
  }
}
