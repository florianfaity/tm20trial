import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';

import { AdminComponent } from './admin.component';
import { admin_routes } from './admin.routes';

@NgModule({
  declarations: [AdminComponent],
  imports: [SharedModule, RouterModule.forChild(admin_routes)],
})
export class AdminModule {}
