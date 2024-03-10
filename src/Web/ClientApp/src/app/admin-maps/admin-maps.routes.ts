import {AdminMapsComponent} from "./admin-maps.component";
import {AdminMapsEditComponent} from "./components/admin-maps-edit/admin-maps-edit.component";
import {AdminMapsListComponent} from "./components/admin-maps-list/admin-maps-list.component";
import {Routes} from "@angular/router";
import {
  AdminMapsSuggestedListComponent
} from "./components/admin-maps-suggested-list/admin-maps-suggested-list.component";

export const adminMaps_routes: Routes = [
  {
    path: '',
    component: AdminMapsComponent,
    children: [
      {
        path: '',
        component: AdminMapsListComponent,
      },
      {
        path: 'suggested',
        component: AdminMapsSuggestedListComponent,
      },
      {
        path: ':id/edit',
        component: AdminMapsEditComponent,
      },
      { path: '', redirectTo: '', pathMatch: 'full' },
      { path: '**', redirectTo: '', pathMatch: 'full' }
    ],
  },
];
