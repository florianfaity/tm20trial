import {AdminMapsComponent} from "./admin-maps.component";
import {AdminMapsEditComponent} from "./components/admin-maps-edit/admin-maps-edit.component";
import {AdminMapsListComponent} from "./components/admin-maps-list/admin-maps-list.component";
import {Routes} from "@angular/router";
import {
  AdminMapsSuggestedListComponent
} from "./components/admin-maps-suggested-list/admin-maps-suggested-list.component";
import {AdminMapsRefusedListComponent} from "./components/admin-maps-refused-list/admin-maps-refused-list.component";

export const adminMaps_routes: Routes = [
  {
    path: '',
    component: AdminMapsComponent,
    children: [
      {
        path: '',
        component: AdminMapsListComponent,
        pathMatch: 'full'
      },
      {
        path: 'suggested',
        component: AdminMapsSuggestedListComponent,
      },
      {
        path: 'refused',
        component: AdminMapsRefusedListComponent,
      },
      {
        path: 'add',
        component: AdminMapsEditComponent,
      },
      {
        path: ':id/edit',
        component: AdminMapsEditComponent,
      },
    ],
  },
];
