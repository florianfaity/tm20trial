import {Component} from "@angular/core";
import {UserDetailsDto, UserDto, UsersClient} from "../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {Location} from "@angular/common";
import {of, switchMap} from "rxjs";

@Component({
  selector: 'app-user-details',
  styles: [`
  `],
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item *ngIf="userDetails != null && userDetails.displayName != null">
              <span nz-icon nzType="user"></span>{{ userDetails.displayName }}</nz-breadcrumb-item>
            <nz-breadcrumb-item *ngIf="userDetails == null || userDetails.displayName == null">
              <span nz-icon nzType="user"></span>Player</nz-breadcrumb-item>
            <nz-breadcrumb-item>Details</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title *ngIf="userDetails != null && userDetails.displayName != null">Detail {{ userDetails.displayName }}</nz-page-header-title>
          <nz-page-header-title *ngIf="userDetails == null || userDetails.displayName == null">Detail player</nz-page-header-title>

          <nz-page-header-extra>
            <nz-space>

              <a *ngIf="userDetails.twitterUsername" href="https://twitter.com/{{userDetails.twitterUsername}}" target="_blank" ><img nz-image width="70px" nzSrc="assets/img/logo-twitter.png" alt="twitter"/></a>
              <a *ngIf="userDetails.twitchUsername" href="https://www.twitch.tv/{{userDetails.twitchUsername}}" target="_blank"><img nz-image width="80px" nzSrc="assets/img/logo-twitch.png" alt="twitch" /></a>
              <a *ngIf="userDetails.twitchUsername" href="https://trackmania.exchange/s/u/{{userDetails.tmxId}}" target="_blank"><img nz-image width="70px" nzSrc="assets/img/logo-trackmania-exchange.png" alt="Trackmania exchange" /></a>

            </nz-space>
          </nz-page-header-extra>
        </nz-page-header>
      </nz-col>
      <nz-col nzSpan="24" *ngIf="!loading; else loadingView">

          <a *ngIf="userDetails.twitterUsername" href="https://twitter.com/{{userDetails.twitterUsername}}" target="_blank" ><img nz-image width="70px" nzSrc="assets/img/logo-twitter.png" alt="twitter"/></a>
          <a *ngIf="userDetails.twitchUsername" href="https://www.twitch.tv/{{userDetails.twitchUsername}}" target="_blank"><img nz-image width="80px" nzSrc="assets/img/logo-twitch.png" alt="twitch" /></a>
          <a *ngIf="userDetails.twitchUsername" href="https://trackmania.exchange/s/u/{{userDetails.tmxId}}" target="_blank"><img nz-image width="70px" nzSrc="assets/img/logo-trackmania-exchange.png" alt="Trackmania exchange" /></a>

        <pre>{{userDetails | json}}</pre>
      </nz-col>
    </nz-row>
    <ng-template #loadingView>
      <app-spinner></app-spinner>
    </ng-template>
  `
})
export class UserDetailsComponent {
  loading = false;
  userDetails: UserDetailsDto;


  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _userClient: UsersClient,
    private _location: Location
  ) {
    this.loading = true;
    const user$ = this._route.params.pipe(
      switchMap(params => {
        if (params['id']) {
          return this._userClient.getUserDetails(params['id']);
        } else {
          return of<UserDetailsDto>(new UserDetailsDto());
        }
      })
    );
    user$.subscribe({
      next: result => {
        this.userDetails = result
        this.loading = false;
      },
      error: error => {
        console.error(error)
        this.loading = false;
      }
    });
  }


}
