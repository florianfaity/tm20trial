import {Component, OnInit} from '@angular/core';
import {of, Subject, switchMap, takeUntil} from "rxjs";
import {LeaderboardsClient, MapDto, MapsClient, UserDetailsDto} from "../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../shared/services/toast.service";
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";
import {AuthorizeService} from "../../api-authorization/authorize.service";

@Component({
  selector: 'app-leaderboard',
  templateUrl: 'leaderboard.component.html',
})
export class LeaderboardComponent implements OnInit{
  loading = false;
  leaderboard: UserDetailsDto[] = [];

  currentBreakpoint: string = '';
  constructor(private leaderboardsClient: LeaderboardsClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService,
              private _nzBreakpoint: NzBreakpointService,
              private _authorizeService: AuthorizeService,
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

}
