import {Routes} from '@angular/router';
import {TrackmaniaComponent} from "./trackmania.component";
import {HomeComponent} from "../home/home.component";
import {LeaderboardComponent} from "../leaderboard/leaderboard.component";
import {MapsComponent} from "../maps/maps.component";
import {IsRoleGuard} from "../../api-authorization/guards/is-role.guard";


export const trackmania_route: Routes = [
  {
    path: '',
    component: TrackmaniaComponent,
    children: [
      {
        path: 'home',
        loadChildren: () => import('../home/home.module').then((m) => m.HomeModule),
        pathMatch: 'full'
      },
      {
        path: 'leaderboard',
        loadChildren: () => import('../leaderboard/leaderboard.module').then((m)  => m.LeaderboardModule)
      },
      {
        path: 'maps',
        loadChildren: () => import('../maps/maps.module').then((m) => m.MapsModule)
      },
      {path: '**', redirectTo: 'home'},
    ],
  },
];
