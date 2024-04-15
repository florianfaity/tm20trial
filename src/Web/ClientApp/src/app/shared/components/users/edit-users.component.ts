import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from "@angular/core";
import {
  CreateMapCommand,
  EStateValidation,
  ETypeTrial,
  UpdateMapCommand,
  UpdateUserCommand,
  UserDto, UsersClient
} from "../../../web-api-client";
import {UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {ToastService} from "../../services/toast.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-edit-users',
  styles: [
    `
      .margin-line {
        margin-top: 5px;
        margin-bottom: 10px;
      }
      .text-center{
        text-align: center;
      }
    `,
  ],
  template: `
      <nz-card nzBorderless>
        <form [formGroup]="form">
          <nz-row>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzRequired nzFor="email">
                  Email
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                    <input type="text" nz-input formControlName="email"/>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzRequired nzFor="displayName">
                  Display name
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                  <input type="text" nz-input formControlName="displayName"/>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzRequired nzFor="tmIoId">
                  User Trackmania Io Id
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                  <input type="text" nz-input formControlName="tmIoId"/>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="loginUplay">
                  Login Uplay
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                  <input type="text" nz-input formControlName="loginUplay"/>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="twitchUsername">
                  Twitch Username
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                  <input type="text" nz-input formControlName="twitchUsername"/>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="twitterUsername">
                  Twitter Username
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                  <input type="text" nz-input formControlName="twitterUsername"/>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
              <nz-row>
                <nz-form-label [nzLg]="8" [nzXs]="24" nzFor="tmxId">
                  Trackmania exchange Id
                </nz-form-label>
                <nz-form-control nz-col [nzLg]="16" [nzXs]="24">
                  <input type="text" nz-input formControlName="tmxId"/>
                </nz-form-control>
              </nz-row>
            </nz-col>
            <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 12 }" [nzXs]="24">
                  <label nz-checkbox formControlName="isMapper">Are you a mapper ?</label>
            </nz-col>

            <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24" style="margin-top: 5px;">
              <nz-row class="margin-line" nzJustify="space-between">
                <nz-col [nzLg]="18" class="text-center">
                  <button nz-button nzType="primary" nzGhost class="margin-button" type="button"
                          (click)="goBack.emit()">
                    <i nz-icon nzType="close" nzTheme="outline"></i> Cancel
                  </button>
                </nz-col>
                <nz-col [nzLg]="6">
                  <button nz-button nzType="primary" class="margin-button-edition" (click)="onSubmit()">
                    <i nz-icon nzType="save" nzTheme="outline"></i>
                    {{ isEdit ? 'Update' : 'Add' }}
                  </button>
                </nz-col>
              </nz-row>
            </nz-col>
          </nz-row>
        </form>
      </nz-card>
  `,
})
export class EditUsersComponent implements OnChanges, OnInit  {
  @Input() user: UserDto;
  @Input() isEdit: boolean;
  @Input() fromAdmin: boolean = false;

  @Output() goBack = new EventEmitter();

  form: UntypedFormGroup = new UntypedFormGroup({});

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    fb: UntypedFormBuilder,
    private _toastService: ToastService,
    private _userClient: UsersClient) {
    this.form = fb.group({
      email: [null, Validators.required],
      displayName: [null, Validators.required],
      tmIoId: [null, Validators.required],
      nation: [],
      loginUplay: [],
      twitchUsername: [],
      twitterUsername: [],
      tmxId: [],
      isMapper: [],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
  }

  ngOnInit(): void {
    this.setValue();
  }

  setValue(){
    if (this.isEdit && this.user?.idUser != null) {
      this.form.get('email').setValue(this.user.email);
      this.form.get('displayName').setValue(this.user.displayName);
      this.form.get('tmIoId').setValue(this.user.tmIoId);
      this.form.get('nation').setValue(this.user.nation);
      this.form.get('loginUplay').setValue(this.user.loginUplay);
      this.form.get('twitchUsername').setValue(this.user.twitchUsername);
      this.form.get('twitterUsername').setValue(this.user.twitterUsername);
      this.form.get('tmxId').setValue(this.user.tmxId);
      this.form.get('isMapper').setValue(this.user.isMapper);
    }
  }

  onSubmit() {
    if (!this.form.valid) {
      return;
    }
    if (this.isEdit) {
      let modelToSubmit = new UpdateUserCommand({
        ...this.form.value,
      })
      modelToSubmit.id = this.user.idUser;
      this._userClient.updateUser(this.user.idUser, modelToSubmit).subscribe({
        next: result => {
          this._toastService.success("User updated")
          if(this.fromAdmin)
            this._router.navigate(['..'], {relativeTo: this._route});
        },
        error: error => {
          console.error(error)
          this._toastService.error("Error")
        }
      });
    }
  }
}
