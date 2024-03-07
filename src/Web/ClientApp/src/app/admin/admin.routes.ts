import { Routes } from '@angular/router';

import { IsRoleGuard } from '../../api-authorization/guards/is-role.guard';

import { AdminComponent } from './admin.component';
import {LeaderboardComponent} from "../leaderboard/leaderboard.component";

export const admin_routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [IsRoleGuard],
    data: { role: 'admin' },
    children: [
      {
        path: 'users',
        pathMatch: 'prefix',
     //   loadChildren: () => import('../admin-/admin-.module').then((m) => m.),
      },
      {
        path: 'maps',
        pathMatch: 'prefix',
        loadChildren: () => import('../admin-maps/admin-maps.module').then((m) => m.AdminMapsModule),
      },
      // {
      //   path: 'records',
      //   pathMatch: 'prefix',
      //  loadChildren: () =>          import('../admin-/admin-.module').then((m) => m.),
      // },

      { path: '**', redirectTo: 'users' },
    ],
  },
];
