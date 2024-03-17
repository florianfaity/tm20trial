import { Injectable } from "@angular/core";
import { UserManager } from "oidc-client";
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
import {CurrentUserDto, UserDto, UsersClient} from "../app/web-api-client";
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

  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);
  constructor(private usersClient: UsersClient) {
  }

  public isAuthenticated(): Observable<boolean> {
    return this.usersClient.getUserRoles().pipe(map((u) => !!u));
  //  return this.getUser().pipe(map((u) => !!u));
  }

  public getUser(): Observable<CurrentUserDto | null> {
    return this.usersClient.getCurrentUser().pipe(map((u) => u));
  }


  // We try to authenticate the user in three different ways:
  // 1) We try to see if we can authenticate the user silently. This happens
  //    when the user is already logged in on the IdP and is done using a hidden iframe
  //    on the client.
  // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
  //    redirect flow.
  // public async signIn(state: any): Promise<IAuthenticationResult> {
  //   await this.ensureUserManagerInitialized();
  //   let user: OidcUser = null;
  //   try {
  //     user = await this.userManager.signinSilent(this.createArguments());
  //     this.userSubject.next(user.profile as unknown as User);
  //     return this.success(state);
  //   } catch (silentError) {
  //     // User might not be authenticated, fallback to popup authentication
  //     console.info('Silent authentication error: ', silentError);
  //
  //     try {
  //       if (this.popUpDisabled) {
  //         throw new Error(
  //           "Popup disabled. Change 'authorize.service.ts:AuthorizeService.popupDisabled' to false to enable it."
  //         );
  //       }
  //       user = await this.userManager.signinPopup(this.createArguments());
  //       this.userSubject.next(user.profile as unknown as User);
  //       return this.success(state);
  //     } catch (popupError) {
  //       if (popupError.message === 'Popup window closed') {
  //         // The user explicitly cancelled the login action by closing an opened popup.
  //         return this.error('The user closed the window.');
  //       } else if (!this.popUpDisabled) {
  //         console.info('Popup authentication error: ', popupError);
  //       }
  //
  //       // PopUps might be blocked by the user, fallback to redirect
  //       try {
  //         await this.userManager.signinRedirect(this.createArguments(state));
  //         return this.redirect();
  //       } catch (redirectError) {
  //         console.info('Redirect authentication error: ', redirectError);
  //         return this.error(redirectError);
  //       }
  //     }
  //   }
  // }


  private userManager: UserManager;

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

}
