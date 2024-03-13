import { Injectable } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable()
export class ToastService {
  constructor(private _notification: NzNotificationService) {}

  public success(content: string, title: string = null, duration?: number) {
    this._notification.success(title, content, duration ? { nzDuration: duration } : null);
  }

  public error(content: string, title: string = 'Error') {
    this._notification.error(title, content);
  }
}
