import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import {AdminUsersComponent} from "./admin-users.component";
import { adminUsers_routes} from "./admin-users.routes";
import {AdminUsersListComponent} from "./components/admin-users-list.component/admin-users-list.component";
import {AdminUsersEditComponent} from "./components/admin-users-edit/admin-users-edit.component";

@NgModule({
  imports: [SharedModule, RouterModule.forChild(adminUsers_routes)],
  declarations: [
    AdminUsersComponent,AdminUsersListComponent,AdminUsersEditComponent
  ],
})
export class AdminUsersModule {}
