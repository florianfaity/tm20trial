import { Routes } from '@angular/router';
import {TrackmaniaComponent} from "./trackmania.component";
import {HomeComponent} from "../home/home.component";
import {LeaderboardComponent} from "../leaderboard/leaderboard.component";
import {MapsComponent} from "../maps/maps.component";


export const trackmania_route: Routes = [
  {
    path: '',
    component: TrackmaniaComponent,
    children: [
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'leaderboard', component: LeaderboardComponent},
      {path: 'maps', component: MapsComponent},
      { path: '**', redirectTo: '' },
    ],
  },
];
