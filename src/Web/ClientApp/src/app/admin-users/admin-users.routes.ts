
import {Routes} from "@angular/router";
import {AdminUsersComponent} from "./admin-users.component";
import {AdminUsersListComponent} from "./components/admin-users-list.component/admin-users-list.component";

export const adminUsers_routes: Routes = [
  {
    path: '',
    component: AdminUsersComponent,
    children: [
      {
        path: '',
        component: AdminUsersListComponent,
      },
      // {
      //   path: 'maps/suggested',
      //   component: AdminMapsSuggestedListComponent,
      // },
      // {
      //   path: 'maps/:id/edit',
      //   component: AdminMapsEditComponent,
      // },
    ],
  },
];
