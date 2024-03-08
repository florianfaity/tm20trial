import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import {LeaderboardComponent} from "./leaderboard.component";
import {leaderboard_routes} from "./leaderboard.routes";



@NgModule({
  declarations: [LeaderboardComponent],
  imports: [
    CommonModule, RouterModule.forChild(leaderboard_routes), SharedModule
  ]
})
export class LeaderboardModule { }
