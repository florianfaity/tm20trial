import {Component} from "@angular/core";
import {CurrentUserDto, UserDto, UsersClient} from "../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../shared/services/toast.service";
import {Location} from "@angular/common";
import {shareReplay} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";
import {AuthorizeService} from "../../../api-authorization/authorize.service";
import {map} from "rxjs/operators";

@Component({
  selector: 'app-user-edit',
  styles: [
    `
      .margin-line {
        margin-top: 5px;
        margin-bottom: 10px;
      }
    `,
  ],
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Player</nz-breadcrumb-item>
            <nz-breadcrumb-item>Edit</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>Edit Player</nz-page-header-title>
        </nz-page-header>
      </nz-col>
      <nz-col nzSpan="24" *ngIf="!loading; else loadingView">
        <app-edit-users [user]="user" [isEdit]="true" (goBack)="cancelEdit()"></app-edit-users>
      </nz-col>
      <nz-col nzSpan="24">
        <nz-card nzTitle="Change password">
          <nz-row>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="password">
                  Password
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                  <nz-input-group [nzSuffix]="suffixTemplate">
                    <input [type]="passwordVisible ? 'text' : 'password'" nz-input [(ngModel)]="password"/>
                  </nz-input-group>
                  <ng-template #suffixTemplate>
                    <span nz-icon [nzType]="passwordVisible ? 'eye-invisible' : 'eye'"
                          (click)="passwordVisible = !passwordVisible">
                    </span>
                  </ng-template>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="password">
                  Confirm Password
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                  <nz-input-group [nzSuffix]="suffixTemplate">
                    <input [type]="confirmPasswordVisible ? 'text' : 'password'" nz-input
                           [(ngModel)]="confirmPassword" (blur)="checkMatchPassword()"/>
                  </nz-input-group>
                  <ng-template #suffixTemplate>
                    <span nz-icon [nzType]="confirmPasswordVisible ? 'eye-invisible' : 'eye'"
                          (click)="confirmPasswordVisible = !confirmPasswordVisible">
                    </span>
                  </ng-template>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col *ngIf="passwordNotMatch" class="margin-line" [nzLg]="{ span: 8, offset: 10 }" [nzXs]="24">
              <nz-alert nzType="error" nzShowIcon nzMessage="The password and confirmation password do not match."></nz-alert>
            </nz-col>
            <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24" style="margin-top: 5px;">
              <nz-row class="margin-line" nzJustify="space-between">
                <nz-col [nzLg]="12">
                </nz-col>
                <nz-col [nzLg]="12">
                  <button nz-button nzType="primary" class="margin-button-edition" [disabled]="passwordNotMatch" (click)="updatePassword()">
                    <i nz-icon nzType="save" nzTheme="outline"></i> Change password
                  </button>
                </nz-col>
              </nz-row>
            </nz-col>
          </nz-row>
        </nz-card>
      </nz-col>
    </nz-row>
    <ng-template #loadingView>
      <app-spinner></app-spinner>
    </ng-template>
  `
})
export class UserEditComponent {
  loading = false;
  user: UserDto;
  passwordVisible = false;
  confirmPasswordVisible = false;
  password?: string;
  confirmPassword?: string;
  passwordNotMatch = false;

  constructor(
    private _usersClient: UsersClient,
    private _toastService: ToastService,
    private _userClient: UsersClient,
    private _location: Location
  ) {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this._usersClient.getCurrentUser().pipe(map((u) => u)).subscribe({
      next: (user: CurrentUserDto) => {
        this.user = new UserDto({
          idUser: user.idUser,
          displayName: user.displayName,
          email: user.email,
          nation: user.nation,
          tmIoId: user.tmIoId,
          tmxId: user.tmxId,
          loginUplay: user.loginUplay,
          twitchUsername: user.twitchUsername,
          twitterUsername: user.twitterUsername,
          isMapper: user.roles.indexOf('Mapper') > -1
        });
        this.loading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.loading = false;
        window.location.href = `Identity/Account/Login`;
      }
    });
  }

  cancelEdit() {
    this.loadData();
  }

  checkMatchPassword(){
    this.passwordNotMatch =  this.password != this.confirmPassword;
  }

  updatePassword() {
    if(this.password != this.confirmPassword)
      return;

  }
}
