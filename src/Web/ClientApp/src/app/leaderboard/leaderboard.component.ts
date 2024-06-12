import {Component, OnInit} from '@angular/core';
import {of, Subject, switchMap, takeUntil} from "rxjs";
import {LeaderboardsClient, MapDto, MapsClient, UserDetailsDto} from "../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../shared/services/toast.service";
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";
import {AuthorizeService} from "../../api-authorization/authorize.service";

@Component({
  selector: 'app-leaderboard',
  styleUrls: ['leaderboard.component.less'],
  templateUrl: 'leaderboard.component.html',
})
export class LeaderboardComponent implements OnInit{
  loading = false;
  leaderboard: UserDetailsDto[] = [];

  currentBreakpoint: string = '';
  constructor(private leaderboardsClient: LeaderboardsClient,
              private _router: Router,
              private _nzBreakpoint: NzBreakpointService,
  ) {
  }

  ngOnInit(): void {
    this.loading = true;
    this.leaderboardsClient.getLeaderboard().subscribe({
      next: result => {
        this.leaderboard = result;
        this.loading = false;
      },
      error: error => {
        console.error(error);
        this.loading = false;
      }
    });

    this._nzBreakpoint.subscribe(gridResponsiveMap)
      .pipe(takeUntil(this.destroy$))
      .subscribe((c) => {
        this.currentBreakpoint = c;
      });

  }

  private readonly destroy$ = new Subject<void>();

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  goToDetailsUser(idUser:number){
    this._router.navigateByUrl('/trial/user/' + idUser);
  }

}
