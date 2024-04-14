import {Routes} from "@angular/router";
import {UserComponent} from "./user.component";
import {UserEditComponent} from "./user-edit/user-edit.component";
import {UserDetailsComponent} from "./user-details/user-details.component";

export const user_route: Routes = [
  {
    path: '',
    component: UserComponent,
    children: [
      {
        path: 'edit',
        component: UserEditComponent,
      },
      {
        path: ':id',
        component: UserDetailsComponent,
      },
    ],
  },
];
