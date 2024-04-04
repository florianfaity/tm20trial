import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';

import {SharedModule} from '../shared/shared.module';
import {MapsComponent} from "./maps.component";
import {maps_routes} from "./maps.routes";
import {MapDetailComponent} from "./map-detail/map-detail.component";
import {MapsListComponent} from "./maps-list/maps-list.component";

@NgModule({
  declarations: [MapsComponent, MapsListComponent, MapDetailComponent],
  imports: [
    CommonModule, RouterModule.forChild(maps_routes), SharedModule
  ]
})
export class MapsModule {
}
