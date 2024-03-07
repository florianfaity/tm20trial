import {Observable, Subscription} from "rxjs";
import {NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router} from "@angular/router";
import {Component, OnDestroy, OnInit} from "@angular/core";
import {map} from "rxjs/operators";
import {AuthorizeService} from "../../api-authorization/authorize.service";

@Component({
  selector: 'app-admin',
  template: `
    <app-nav-menu
      [isAdminView]="true"
      [isAdmin]="isAdmin$ | async"
      [isMapper]="isMapper$ | async"
      [playerName]="userName | async"
    >
      <router-outlet></router-outlet>
    </app-nav-menu>
  `,
})
export class AdminComponent implements OnInit, OnDestroy  {

  public routerLoading = false;
  public userName: Observable<string>;
  public isAdmin$: Observable<boolean>;
  public isMapper$: Observable<boolean>;
  public userId$ : Observable<number>;

  private _routeSubs: Subscription;
  constructor(
    private _authorizeService: AuthorizeService,
    private _router: Router,) {}

  ngOnInit(): void {

    this.userName = this._authorizeService.getUser().pipe(map((u) => u && (u.name)));

    this.userId$ = this._authorizeService.getUser().pipe(map((u) => u && u.UserId));

    this.isAdmin$ = this._authorizeService.getUser().pipe(map((u) => u.isAdministrator()));

    this.isMapper$ = this._authorizeService.getUser().pipe(map((u) => u.isMapper()));

    this._routeSubs = this._router.events.subscribe((event) => {
      switch (true) {
        case event instanceof NavigationStart:
          this.routerLoading = true;
          break;
        case event instanceof NavigationEnd:
        case event instanceof NavigationCancel:
        case event instanceof NavigationError:
          this.routerLoading = false;
          break;
      }
    });
  }

  ngOnDestroy(): void {
    if (this._routeSubs) {
      this._routeSubs.unsubscribe();
    }
  }
}
