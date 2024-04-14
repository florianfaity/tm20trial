import {Component} from "@angular/core";
import {UserDto, UsersClient} from "../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {Location} from "@angular/common";

@Component({
  selector: 'app-user-details',
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Player</nz-breadcrumb-item>
             <nz-breadcrumb-item>Details</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>Detail Player </nz-page-header-title>
        </nz-page-header>
      </nz-col>
      <nz-col nzSpan="24" *ngIf="!loading; else loadingView">

       </nz-col>
    </nz-row>
    <ng-template #loadingView>
      <app-spinner></app-spinner>
    </ng-template>
  `
})
export class UserDetailsComponent {
  loading = false;
  user: UserDto;


  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _userClient: UsersClient,
    private _location: Location
  ) {
  }


}
