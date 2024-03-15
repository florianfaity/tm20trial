import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, shareReplay, tap } from 'rxjs/operators';

import { AuthorizeService } from '../authorize.service';
import {CurrentUserDto} from "../../app/web-api-client";

@Injectable({ providedIn: 'root' })
export class IsRoleGuard {
  constructor(private _authorizeService: AuthorizeService, private router: Router) {}

  canActivate(next: ActivatedRouteSnapshot): Observable<boolean> {
    return this._authorizeService.getUser().pipe(
      shareReplay(),
      map((u) => ({ hasRole: this.hasRole(u, next.data.role), redirectUrl: '/trial' })),
      tap((x) => (x.hasRole ? x.hasRole : this.router.navigateByUrl(x.redirectUrl, { replaceUrl: true }))),
      map((x) => x.hasRole)
    );
  }

  private hasRole(user: CurrentUserDto, roleName: string): boolean {
    if (!user) {
      return false;
    }
    if (roleName === 'admin') {
      return user.roles.indexOf('Administrator') >= 0;
    }
    if (roleName === 'mapper') {
      return user.roles.indexOf('Mapper') >= 0;
    }
    if (roleName === 'player') {
      return user.roles.indexOf('Player') >= 0;
    }
    return false;
  }


}
