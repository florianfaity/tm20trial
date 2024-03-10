import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import {AdminMapsComponent} from "./admin-maps.component";
import {AdminMapsEditComponent} from "./components/admin-maps-edit/admin-maps-edit.component";
import {adminMaps_routes} from "./admin-maps.routes";
import {AdminMapsListComponent} from "./components/admin-maps-list/admin-maps-list.component";
import {
  AdminMapsSuggestedListComponent
} from "./components/admin-maps-suggested-list/admin-maps-suggested-list.component";

@NgModule({
  imports: [SharedModule, RouterModule.forChild(adminMaps_routes)],
  declarations: [
    AdminMapsComponent,
    AdminMapsEditComponent,
    AdminMapsListComponent,
    AdminMapsSuggestedListComponent
  ],
})
export class AdminMapsModule {}
