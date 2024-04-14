import {NgModule} from "@angular/core";
import {SharedModule} from "../shared/shared.module";
import {RouterModule} from "@angular/router";
import {AdminRecordsComponent} from "./admin-records.component";
import {adminRecords_routes} from "./admin-records.routes";
import {AdminRecordsListComponent} from "./components/admin-records-list/admin-records-list.component";

@NgModule({
  imports: [SharedModule, RouterModule.forChild(adminRecords_routes)],
  declarations: [
    AdminRecordsComponent,
    AdminRecordsListComponent,
  ],
})
export class AdminRecordsModule {}
