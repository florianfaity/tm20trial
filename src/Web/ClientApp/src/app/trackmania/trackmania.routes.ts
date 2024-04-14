import {Routes} from '@angular/router';
import {TrackmaniaComponent} from "./trackmania.component";

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
      {
        path: 'user',
        loadChildren: () => import('../user/user.module').then((m) => m.UserModule)
      },
      {path: '**', redirectTo: 'home'},
    ],
  },
];
