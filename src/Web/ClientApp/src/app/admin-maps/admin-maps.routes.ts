import {AdminMapsComponent} from "./admin-maps.component";
import {AdminMapsEditComponent} from "./components/admin-maps-edit/admin-maps-edit.component";
import {AdminMapsListComponent} from "./components/admin-maps-list/admin-maps-list.component";
import {Routes} from "@angular/router";

export const adminMaps_routes: Routes = [
  {
    path: '',
    component: AdminMapsComponent,
    children: [
      {
        path: 'maps',
        component: AdminMapsListComponent,
      },
      {
        path: 'maps/:id/edit',
        component: AdminMapsEditComponent,
      },
    ],
  },
];
