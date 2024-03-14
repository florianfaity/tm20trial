
import {Routes} from "@angular/router";
import {AdminUsersComponent} from "./admin-users.component";
import {AdminUsersListComponent} from "./components/admin-users-list.component/admin-users-list.component";
import {AdminUsersEditComponent} from "./components/admin-users-edit/admin-users-edit.component";

export const adminUsers_routes: Routes = [
  {
    path: '',
    component: AdminUsersComponent,
    children: [
      {
        path: '',
        component: AdminUsersListComponent,
      },
      {
        path: 'add',
        component: AdminUsersEditComponent,
      },
      {
        path: ':id/edit',
        component: AdminUsersEditComponent,
      },
    ],
  },
];
