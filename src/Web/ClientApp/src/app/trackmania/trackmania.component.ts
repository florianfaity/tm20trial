import {Component, OnDestroy, OnInit} from "@angular/core";
import {Observable, shareReplay, Subscription} from "rxjs";
import {AuthorizeService} from "../../api-authorization/authorize.service";
import {DomSanitizer} from "@angular/platform-browser";
import {NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router} from "@angular/router";
import {CurrentUserDto} from "../web-api-client";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-trackmania',
  styles: [`
      ::-webkit-scrollbar {
        width: 0.5rem;
        height: 0.5rem;
      }
      ::-webkit-scrollbar-track {
        border-radius: 10px;
      }
      ::-webkit-scrollbar-thumb {
        background: #f0f2f5;
        border-radius: 10px;
      }
      ::-webkit-scrollbar-thumb:hover {
        background: #f0f2f5;
      }
  `],
  template: `
    <app-nav-menu
      [isAdminView]="false"
      [isAdmin]="isAdmin"
      [isMapper]="isMapper"
      [isPlayer]="isPlayer"
      [isConnected]="isConnected"
      [playerName]="userName"
      [idUser]="idUser"
    >
      <router-outlet></router-outlet>
    </app-nav-menu>
  `,
})
export class TrackmaniaComponent implements OnInit, OnDestroy {

  public routerLoading = false;
//  public userName: Observable<string>;
  // public isAdmin$: Observable<boolean>;
  // public isMapper$: Observable<boolean>;
  // public isPlayer$: Observable<boolean>;
  // public userId$: Observable<number>;
  private _routeSubs: Subscription;

  constructor(
    private _authorizeService: AuthorizeService,
    public sanitizer: DomSanitizer,
    private _router: Router,) {
  }

  isAdmin: boolean = false;
  isMapper: boolean = false;
  isPlayer: boolean = false;
  isConnected: boolean = false;
  userName: string = '';
  idUser: number ;

  ngOnInit(): void {

    this._authorizeService.getUser().pipe(shareReplay()).subscribe({
      next: (user: CurrentUserDto) => {
        this.isAdmin = user.roles.indexOf('Administrator') > -1;
        this.isMapper = user.roles.indexOf('Mapper') > -1;
        this.isPlayer = user.roles.indexOf('Player') > -1;
        this.isConnected = this.isAdmin || this.isMapper || this.isPlayer;
        this.userName = user.displayName;
        this.idUser = user.idUser;
      },
      error: (err: HttpErrorResponse) => {
        if (err.status == 401) {
          this.isAdmin = false;
          this.isMapper = false;
          this.isPlayer = false;
          this.isConnected = false;
        }
      }
    });


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
