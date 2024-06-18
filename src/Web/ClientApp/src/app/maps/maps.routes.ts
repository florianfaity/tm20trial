import {Routes} from "@angular/router";
import {MapsComponent} from "./maps.component";
import {MapsListComponent} from "./maps-list/maps-list.component";
import {MapDetailComponent} from "./map-detail/map-detail.component";
import {MapsSuggestComponent} from "./maps-suggest/maps-suggest.component";

export const maps_routes: Routes = [
  {
    path: '',
    component: MapsComponent,
    children: [
      {
        path: '',
        component: MapsListComponent,
        pathMatch: 'full'
      },
      {
        path: 'suggest',
        component: MapsSuggestComponent,
      },
      {
        path: ':id',
        component: MapDetailComponent,
      },
      {path: '**', redirectTo: ''},
    ],
  },
];
