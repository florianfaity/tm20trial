import { Component } from '@angular/core';

@Component({
  selector: 'app-spinner',
  styles: [
    `
      nz-spin {
        margin-top: 15px;
        margin-bottom: 15px;
        padding-top: 50px;
      }
    `,
  ],
  template: `<nz-spin nzSize="large" nzTip="Loading..."></nz-spin>`,
})
export class SpinnerComponent {}
