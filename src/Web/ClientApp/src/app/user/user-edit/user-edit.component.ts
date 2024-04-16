import {Component} from "@angular/core";
import {CurrentUserDto, UpdateUserCommand, UpdateUserPasswordCommand, UserDto, UsersClient} from "../../web-api-client";
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
      <nz-col nzSpan="24">
        <nz-card>

          <nz-tabset nzCentered>
            <nz-tab nzTitle="Appearance ">
              <nz-col nzSpan="24" *ngIf="!loading; else loadingView">
                <app-edit-users [user]="user" [isEdit]="true" (goBack)="cancelEdit()"></app-edit-users>
              </nz-col>
            </nz-tab>
            <nz-tab nzTitle="Change password">
              <nz-col nzSpan="24">
                <nz-row>
                  <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                    <nz-row>
                      <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="password">
                        Old Password
                      </nz-form-label>
                      <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                        <nz-input-group [nzSuffix]="suffixTemplateOld">
                          <input [type]="oldPasswordVisible ? 'text' : 'password'" nz-input [(ngModel)]="oldPassword"/>
                        </nz-input-group>
                        <ng-template #suffixTemplateOld>
                    <span nz-icon [nzType]="oldPasswordVisible ? 'eye-invisible' : 'eye'"
                          (click)="oldPasswordVisible = !oldPasswordVisible">
                    </span>
                        </ng-template>
                      </nz-form-control>
                    </nz-row>
                  </nz-col>
                  <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                    <nz-row>
                      <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="password">
                        New Password
                      </nz-form-label>
                      <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                        <nz-input-group [nzSuffix]="suffixTemplateNew">
                          <input [type]="passwordVisible ? 'text' : 'password'" nz-input [(ngModel)]="password"  (blur)="checkNewPassword()"/>
                        </nz-input-group>
                        <ng-template #suffixTemplateNew>
                    <span nz-icon [nzType]="passwordVisible ? 'eye-invisible' : 'eye'"
                          (click)="passwordVisible = !passwordVisible">
                    </span>
                        </ng-template>
                      </nz-form-control>
                    </nz-row>
                  </nz-col>
                  <nz-col *ngIf="!passwordControlValid" class="margin-line" [nzLg]="{ span: 8, offset: 10 }" [nzXs]="24">
                    <nz-alert nzType="error" nzShowIcon
                              nzMessage="The password must have at least one non alphanumeric character, one digit, one lowercase, one uppercase and at least 6 characters."></nz-alert>
                  </nz-col>
                  <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                    <nz-row>
                      <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="password">
                        Confirm new Password
                      </nz-form-label>
                      <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                        <nz-input-group [nzSuffix]="suffixTemplateConfirm">
                          <input [type]="confirmPasswordVisible ? 'text' : 'password'" nz-input
                                 [(ngModel)]="confirmPassword" (blur)="checkMatchPassword()"/>
                        </nz-input-group>
                        <ng-template #suffixTemplateConfirm>
                    <span nz-icon [nzType]="confirmPasswordVisible ? 'eye-invisible' : 'eye'"
                          (click)="confirmPasswordVisible = !confirmPasswordVisible">
                    </span>
                        </ng-template>
                      </nz-form-control>
                    </nz-row>
                  </nz-col>
                  <nz-col *ngIf="passwordNotMatch" class="margin-line" [nzLg]="{ span: 8, offset: 10 }" [nzXs]="24">
                    <nz-alert nzType="error" nzShowIcon
                              nzMessage="The password and confirmation password do not match."></nz-alert>
                  </nz-col>
                  <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24" style="margin-top: 5px;">
                    <nz-row class="margin-line" nzJustify="space-between">
                      <nz-col [nzLg]="12">
                      </nz-col>
                      <nz-col [nzLg]="12">
                        <button nz-button nzType="primary" class="margin-button-edition" [disabled]="passwordNotMatch || !passwordControlValid"
                                (click)="updatePassword()">
                          <i nz-icon nzType="save" nzTheme="outline"></i> Change password
                        </button>
                      </nz-col>
                    </nz-row>
                  </nz-col>
                </nz-row>
              </nz-col>
            </nz-tab>
          </nz-tabset>
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
  oldPasswordVisible = false;
  oldPassword?: string;

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

  passwordControlValid = true;

  checkNewPassword(){
    this.passwordControlValid = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/.test(this.password);
  }

  checkMatchPassword() {
    this.passwordNotMatch = this.password != this.confirmPassword;
  }

  updatePassword() {
    if (this.password != this.confirmPassword)
      return;


    let modelToSubmit = new UpdateUserPasswordCommand({
      currentPassword: this.oldPassword,
      newPassword: this.password,
      confirmPassword: this.confirmPassword,
    })
    this._userClient.updateUserPassword(this.user.idUser, modelToSubmit).subscribe({
      next: result => {
        this._toastService.success("User password updated");
        this.initPassword();
      },
      error: error => {
        console.error(error)
        this._toastService.error("Error")
        this.initPassword();
      }
    });
  }
  initPassword()
  {
    this.password = "";
    this.oldPassword = "";
    this.confirmPassword = "";
    this.confirmPasswordVisible = false;
    this.passwordVisible = false;
    this.oldPasswordVisible = false;
  }
}
