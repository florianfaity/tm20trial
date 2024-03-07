import { Injectable } from "@angular/core";
import {
  BehaviorSubject,
  concat,
  distinctUntilChanged,
  filter,
  from,
  mergeMap,
  Observable,
  shareReplay,
  take,
  tap
} from "rxjs";
import {map} from "rxjs/operators";
import { User as OidcUser, UserManager } from 'oidc-client';
import {ApplicationName, ApplicationPaths} from "./api-authorization.constants";

export type IAuthenticationResult =
  | SuccessAuthenticationResult
  | FailureAuthenticationResult
  | RedirectAuthenticationResult;

export interface SuccessAuthenticationResult {
  status: AuthenticationResultStatus.Success;
  state: any;
}

export interface FailureAuthenticationResult {
  status: AuthenticationResultStatus.Fail;
  message: string;
}

export interface RedirectAuthenticationResult {
  status: AuthenticationResultStatus.Redirect;
}

export enum AuthenticationResultStatus {
  Success,
  Redirect,
  Fail,
}


export interface IUser {
  name: string;
  UserId: number;
  role: string[];
  email: string;
  IdentityId: string;

  isAdministrator(): boolean;
  isMapper(): boolean;
  isPlayer(): boolean;
}

export class User implements IUser {
  UserId: number;
  name: string;
  role: string[] = [];
  email: string;
  IdentityId: string;

  isPlayer(): boolean {
    return this.isMapper() || this.isAdministrator() || this.hasRole('Player');
  }

  isMapper() : boolean {
    return this.isAdministrator() || this.hasRole('Mapper');
  }

  isAdministrator(): boolean {
    return this.hasRole('Administrator');
  }

  private hasRole(name: string) {
    return this.role && this.role.indexOf(name) >= 0;
  }
}

@Injectable({
  providedIn: 'root',
})
export class AuthorizeService {
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.

  private popUpDisabled = true;
  private userManager: UserManager;
  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);

  constructor() {
  }

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map((u) => !!u));
  }

  public getUser(): Observable<IUser | null> {
    return concat(
      this.userSubject.pipe(
        take(1),
        filter((u) => !!u)
      ),
      this.getUserFromStorage().pipe(
        filter((u) => !!u),
        tap((u) => this.userSubject.next(u))
      ),
      this.userSubject.asObservable()
    ).pipe(
      shareReplay(1),
      distinctUntilChanged(),
      map((u) => {
        if (u) {
          const b: User = new User();
          Object.assign(b, u);
          return b;
        }
        return u;
      })
    );
  }

  private async ensureUserManagerInitialized(): Promise<void> {
    if (this.userManager !== undefined) {
      return;
    }

    const response = await fetch(ApplicationPaths.ApiAuthorizationClientConfigurationUrl);
    if (!response.ok) {
      throw new Error(`Could not load settings for '${ApplicationName}'`);
    }

    const settings: any = await response.json();
    settings.automaticSilentRenew = true;
    settings.includeIdTokenInSilentRenew = true;
    this.userManager = new UserManager(settings);

    this.userManager.events.addUserSignedOut(async () => {
      await this.userManager.removeUser();
      this.userSubject.next(null);
    });
  }

  private getUserFromStorage(): Observable<IUser> {
    return from(this.ensureUserManagerInitialized()).pipe(
      mergeMap(() => this.userManager.getUser()),
      map((u) => u && u.profile),
      map((u) => {
        const x = u as unknown as User;

        if (u) {
          x.IdentityId = u.sub;
        }

        return x;
      })
    );
  }
}
