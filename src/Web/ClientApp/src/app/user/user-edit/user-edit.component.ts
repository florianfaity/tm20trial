import {Component} from "@angular/core";
import {UserDto, UsersClient} from "../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../shared/services/toast.service";
import {Location} from "@angular/common";

@Component({
  selector: 'app-user-edit',
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header nzBackIcon [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Player</nz-breadcrumb-item>
             <nz-breadcrumb-item>Edit</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>Edit Player</nz-page-header-title>
        </nz-page-header>
      </nz-col>
      <nz-col nzSpan="24" *ngIf="!loading; else loadingView">
         <app-edit-users [user]="user" [isEdit]="true" (goBack)="goBack()"></app-edit-users>
      </nz-col>
    </nz-row>
    <ng-template #loadingView>
      <app-spinner></app-spinner>
    </ng-template>
  `
})
export class UserEditComponent {
  loading = false;
  user: UserDto;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _toastService: ToastService,
    private _userClient: UsersClient,
    private _location: Location
  ) {
  }

  goBack(){
    
  }
}
