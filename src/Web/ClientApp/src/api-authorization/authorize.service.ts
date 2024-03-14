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
import {CurrentUserDto, UserDto, UsersClient} from "../app/web-api-client";

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
//
//
// export interface IUser {
//   name: string;
//   UserId: number;
//   role: string[];
//   email: string;
//   IdentityId: string;
//
//   isAdministrator(): boolean;
//   isMapper(): boolean;
//   isPlayer(): boolean;
// }
//
// export class User implements IUser {
//   UserId: number;
//   name: string;
//   role: string[] = [];
//   email: string;
//   IdentityId: string;
//
//   isPlayer(): boolean {
//     return this.isMapper() || this.isAdministrator() || this.hasRole('Player');
//   }
//
//   isMapper() : boolean {
//     return this.isAdministrator() || this.hasRole('Mapper');
//   }
//
//   isAdministrator(): boolean {
//     return this.hasRole('Administrator');
//   }
//
//   private hasRole(name: string) {
//     return this.role && this.role.indexOf(name) >= 0;
//   }
// }

@Injectable({
  providedIn: 'root',
})
export class AuthorizeService {
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.

  constructor(private usersClient: UsersClient) {
  }

  public isAuthenticated(): Observable<boolean> {
    return this.usersClient.getUserRoles().pipe(map((u) => !!u));
  //  return this.getUser().pipe(map((u) => !!u));
  }

  public getUser(): Observable<CurrentUserDto | null> {
    return this.usersClient.getCurrentUser().pipe(map((u) => u));
  }


  // private getUserFromStorage(): Observable<IUser> {
  //   return from(this.ensureUserManagerInitialized()).pipe(
  //     mergeMap(() => this.userManager.getUser()),
  //     map((u) => u && u.profile),
  //     map((u) => {
  //       const x = u as unknown as User;
  //
  //       if (u) {
  //         x.IdentityId = u.sub;
  //       }
  //
  //       return x;
  //     })
  //   );
  // }
}
