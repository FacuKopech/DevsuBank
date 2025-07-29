import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ResponseHandlerService {
  private _messageTitle = new BehaviorSubject<string>('');
  private _message = new BehaviorSubject<string>('');
  private _isError = new BehaviorSubject<boolean>(false);
  private _showMessageModal = new BehaviorSubject<boolean>(false);

  messageTitle$ = this._messageTitle.asObservable();
  message$ = this._message.asObservable();
  isError$ = this._isError.asObservable();
  showMessageModal$ = this._showMessageModal.asObservable();

  handleResponse(result: { success: boolean, message: string }) {
    this._messageTitle.next(result.success ? 'Excelente' : 'Error');
    this._message.next(result.message);
    this._isError.next(!result.success);
    this._showMessageModal.next(true);
  }

  closeMessageModal() {
    this._showMessageModal.next(false);
  }
}
