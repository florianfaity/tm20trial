import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';

import { AdminComponent } from './admin.component';
import { admin_routes } from './admin.routes';
import {AppModule} from "../app.module";

@NgModule({
  declarations: [AdminComponent],
  imports: [SharedModule, RouterModule.forChild(admin_routes), AppModule],
})
export class AdminModule {}
