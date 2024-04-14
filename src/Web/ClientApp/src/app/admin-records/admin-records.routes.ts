import {Routes} from "@angular/router";
import {AdminRecordsComponent} from "./admin-records.component";
import {AdminRecordsListComponent} from "./components/admin-records-list/admin-records-list.component";

export const adminRecords_routes: Routes = [
  {
    path: '',
    component: AdminRecordsComponent,
    children: [
      {
        path: '',
        component: AdminRecordsListComponent,
        pathMatch: 'full'
      },
    ],
  },
];
