import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import {HomeComponent} from "./home.component";
import {home_routes} from "./home.routes";



@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule, RouterModule.forChild(home_routes), SharedModule
  ]
})
export class HomeModule { }
