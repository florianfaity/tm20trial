import {Component} from "@angular/core";
import {
  EStateValidation,
  MapDto,
  MapsClient,
  RecordDto,
  UserDetailsDto,
  UserDto,
  UsersClient
} from "../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {DatePipe, Location} from "@angular/common";
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
          <nz-page-header-extra *ngIf="userDetails != null">
            <nz-space>
              <a *ngIf="userDetails.twitterUsername" href="https://twitter.com/{{userDetails.twitterUsername}}" target="_blank" ><img nz-image width="70px" nzSrc="assets/img/logo-twitter.png" alt="twitter"/></a>
              <a *ngIf="userDetails.twitchUsername" href="https://www.twitch.tv/{{userDetails.twitchUsername}}" target="_blank"><img nz-image width="80px" nzSrc="assets/img/logo-twitch.png" alt="twitch" /></a>
              <a *ngIf="userDetails.twitchUsername" href="https://trackmania.exchange/s/u/{{userDetails.tmxId}}" target="_blank"><img nz-image width="70px" nzSrc="assets/img/logo-trackmania-exchange.png" alt="Trackmania exchange" /></a>
            </nz-space>
          </nz-page-header-extra>
        </nz-page-header>
      </nz-col>
      <nz-col nzSpan="24" *ngIf="!loading; else loadingView">
        <nz-row [nzGutter]="[16, 16]">
          <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
            <nz-row>
              <nz-card nzBorderless style="width: 100%;">
                <nz-row>
                  <nz-col nzSpan="12">Points :</nz-col>
                  <nz-col nzSpan="12">{{ userDetails.points }}</nz-col>
                </nz-row>
                <nz-row>
                  <nz-col nzSpan="12">Maps finished :</nz-col>
                  <nz-col nzSpan="12">{{ userDetails.numberOfMapCompleted }} / {{ maps.length }}</nz-col>
                </nz-row>
              </nz-card>
            </nz-row>
          </nz-col>
          <nz-col [nzLg]="{ span: 18, offset: 3 }" [nzXs]="24">
            <nz-row>
              <nz-card nzBorderless style="width: 100%;">
                <nz-row>
                  <nz-col nzSpan="24">
                    <nz-table #nzTable [nzData]="maps"  [nzFrontPagination]="false" [nzShowPagination]="false"
                              [nzLoading]="loading" nzSize="small">
                      <thead>
                      <tr>
                        <th>Name</th>
                        <th nzAlign="center">Difficulty</th>
                        <th nzAlign="center">Points</th>
                        <th nzAlign="center">Time</th>
                        <th nzAlign="center">Submitted</th>
                      </tr>
                      </thead>
                      <tbody>
                      <tr *ngFor="let map of nzTable.data">
                        <td>{{ map.name }}</td>
                        <td nzAlign="center">{{ map.difficulty | difficulty }}</td>
                        <td nzAlign="center">{{ map.points }}</td>
                        <td nzAlign="center">
                          <label *ngIf="getRecordTimeByMap(map.id) == null">-</label>
                          <a nz-button  *ngIf="getRecordTimeByMap(map.id) != null"
                             nzType="link" [href]="getLinkRecordByMap(map.id)"> {{ getRecordTimeByMap(map.id) }}
                          </a>
                        </td>
                        <td nzAlign="center">
                          {{ getRecordDateSubmittedByMap(map.id) }}
                        </td>
                      </tr>
                      </tbody>
                    </nz-table>
                  </nz-col>
                </nz-row>
              </nz-card>
            </nz-row>
          </nz-col>
        </nz-row>
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
  maps: MapDto[];

  constructor(
    private _route: ActivatedRoute,
    private mapsClient: MapsClient,
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

    mapsClient.getMaps(EStateValidation.Validate).subscribe({
      next: result => {
        this.maps = result.sort((a, b) => a.points - b.points);
        this.loading = false;
      },
      error: error => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  getRecordTimeByMap(idMap): string{
    var record = this.userDetails.records.find(r => r.idMap == idMap);
    return record != null? record.time : null;
  }

  getRecordDateSubmittedByMap(idMap): string{
    var record = this.userDetails.records.find(r => r.idMap == idMap);
    if(record == null)
      return '-';

    const datepipe: DatePipe = new DatePipe('en-US')

    return datepipe.transform(record.datePersonalBest, 'dd-MM-YYYY HH:mm:ss');
  }

  getLinkRecordByMap(idMap): string{
    var record = this.userDetails.records.find(r => r.idMap == idMap);
    return record != null? record.fileUrl.replace('https://core.trackmania.nadeo.live/storageObjects','https://trackmania.io/api/download/ghost') : '-';
  }
}
