import {Component, OnDestroy, OnInit} from "@angular/core";
import {Observable, shareReplay, Subscription} from "rxjs";
import {AuthorizeService, IUser} from "../../api-authorization/authorize.service";
import {map} from "rxjs/operators";
import {DomSanitizer} from "@angular/platform-browser";
import {NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router} from "@angular/router";

@Component({
  selector: 'app-trackmania',
  template: `
    <app-nav-menu
      [isAdminView]="false"
      [isAdmin]="isAdmin$ | async"
      [isMapper]="isMapper$ | async"
      [isPlayer]="isPlayer$ | async"
      [playerName]="userName | async"
    >
      <router-outlet></router-outlet>
    </app-nav-menu>
  `,
})
export class TrackmaniaComponent implements OnInit, OnDestroy {

  public routerLoading = false;
  public userName: Observable<string>;
  public isAdmin$: Observable<boolean>;
  public isMapper$: Observable<boolean>;
  public isPlayer$: Observable<boolean>;
  public userId$ : Observable<number>;
  private _routeSubs: Subscription;

  constructor(
    private _authorizeService: AuthorizeService,
    public sanitizer: DomSanitizer,
    private _router: Router,) {}

  ngOnInit(): void {

    const user$ = this._authorizeService.getUser().pipe(shareReplay());

    this.userName = user$.pipe(map((u : IUser) => u && (u.name)));

    this.isAdmin$ = user$.pipe(map((u : IUser) => u.isAdministrator()));
    this.isMapper$ = user$.pipe(map((u : IUser) => u.isMapper()));
    this.isPlayer$ = user$.pipe(map((u : IUser) => u.isPlayer()));

    this.userId$ = this._authorizeService.getUser().pipe(map((u) => u && u.UserId));


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
