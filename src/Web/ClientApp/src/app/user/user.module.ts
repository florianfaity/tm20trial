import {NgModule} from "@angular/core";
import {SharedModule} from "../shared/shared.module";
import {RouterModule} from "@angular/router";
import {UserComponent} from "./user.component";
import {user_route} from "./user.routes";
import {UserEditComponent} from "./user-edit/user-edit.component";
import {UserDetailsComponent} from "./user-details/user-details.component";

@NgModule({
  declarations: [UserComponent, UserEditComponent, UserDetailsComponent],
  imports: [SharedModule, RouterModule.forChild(user_route)],
})
export class UserModule {}
