import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import {MapsComponent} from "./maps.component";
import {maps_routes} from "./maps.routes";



@NgModule({
  declarations: [MapsComponent],
  imports: [
    CommonModule, RouterModule.forChild(maps_routes), SharedModule
  ]
})
export class MapsModule { }
