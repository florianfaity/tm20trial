import {Observable, shareReplay, Subscription} from "rxjs";
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
    >
    </app-nav-menu>
    <router-outlet></router-outlet>

<!--      -->
  `,
})
export class AdminComponent implements OnInit, OnDestroy  {

  public routerLoading = false;

 userName: string;

  private _routeSubs: Subscription;
  constructor(
    private _authorizeService: AuthorizeService,
    private _router: Router,) {}

  ngOnInit(): void {
    console.log("admin")
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
  }

  ngOnDestroy(): void {
    if (this._routeSubs) {
      this._routeSubs.unsubscribe();
    }
  }
}
