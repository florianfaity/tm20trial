import {SharedModule} from "../shared/shared.module";
import {NgModule} from "@angular/core";
import {TrackmaniaComponent} from "./trackmania.component";
import {RouterModule} from "@angular/router";
import {trackmania_route} from "./trackmania.routes";

@NgModule({
  declarations: [TrackmaniaComponent],
  imports: [SharedModule, RouterModule.forChild(trackmania_route)],
})
export class TrackmaniaModule {}
