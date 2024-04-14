import {Observable, of, shareReplay, Subscription, switchMap, tap} from "rxjs";
import {NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router} from "@angular/router";
import {Component, OnDestroy, OnInit} from "@angular/core";
import {map} from "rxjs/operators";
import {AuthorizeService} from "../../api-authorization/authorize.service";
import {CurrentUserDto} from "../web-api-client";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-admin',
  template: `
    <app-nav-menu
      [isAdminView]="true"
      [isAdmin]="true"
      [isConnected]="true"
      [playerName]="userName"
      [idUser]="idUser"
    >
      <router-outlet></router-outlet>
    </app-nav-menu>
  `,
})
export class AdminComponent implements OnDestroy  {

  public routerLoading = false;
  isAuthenticated$: Observable<boolean>;
  loading = false;

 userName: string;
 idUser: number;

  private _routeSubs: Subscription;
  constructor(
    private _authorizeService: AuthorizeService,
    private _router: Router,) {
    this.loading = true;

    this._authorizeService.getUser().pipe(shareReplay()).subscribe({
      next: (user: CurrentUserDto) => {
        this.userName = user.displayName;
        this.idUser = user.idUser;
      },
      error: (err: HttpErrorResponse) =>console.error(err)
    });

    this.isAuthenticated$ = this._authorizeService.isAuthenticated().pipe(
      tap(() => (this.loading = false)),
      switchMap((authenticated) => {
        if (authenticated) {
          return of(authenticated);
        } else {
          return this._router.navigateByUrl(`/trial`);
        //  return this._router.navigateByUrl(`/authentication/login?action=login&returnUrl=/`);
        }
      })
    );

  }

  // ngOnInit(): void {
  //   console.log("admin")
    // this._authorizeService.getUser().pipe(shareReplay()).subscribe({
    //   next: (user: CurrentUserDto) => {
    //     if(!user.roles.indexOf('Administrator')){
    //
    //     }
    //
    //   },
    //   error : (err: HttpErrorResponse) => {
    //
    //   }
    // });
    //
    // this._routeSubs = this._router.events.subscribe((event) => {
    //   switch (true) {
    //     case event instanceof NavigationStart:
    //       this.routerLoading = true;
    //       break;
    //     case event instanceof NavigationEnd:
    //     case event instanceof NavigationCancel:
    //     case event instanceof NavigationError:
    //       this.routerLoading = false;
    //       break;
    //   }
    // });
  // }

  ngOnDestroy(): void {
    if (this._routeSubs) {
      this._routeSubs.unsubscribe();
    }
  }
}
