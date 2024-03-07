import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, shareReplay, tap } from 'rxjs/operators';

import { AuthorizeService, IUser } from '../authorize.service';

@Injectable({ providedIn: 'root' })
export class IsRoleGuard {
  constructor(private _authorizeService: AuthorizeService, private router: Router) {}

  canActivate(next: ActivatedRouteSnapshot): Observable<boolean> {
    return this._authorizeService.getUser().pipe(
      shareReplay(),
      map((u) => ({ hasRole: this.hasRole(u, next.data.role), redirectUrl: '/' })),
      tap((x) => (x.hasRole ? x.hasRole : this.router.navigateByUrl(x.redirectUrl, { replaceUrl: true }))),
      map((x) => x.hasRole)
    );
  }

  private hasRole(user: IUser, roleName: string): boolean {
    if (!user) {
      return false;
    }

    switch (roleName) {
      case 'admin':
        return user.isAdministrator();
      case 'mapper':
        return user.isMapper();
      case 'player':
        return user.isPlayer();
      default:
        return false;
    }
  }

}
