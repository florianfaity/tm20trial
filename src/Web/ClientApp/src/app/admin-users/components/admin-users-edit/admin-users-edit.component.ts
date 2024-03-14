import {Component, OnChanges, SimpleChanges} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../../shared/services/toast.service";
import {of, switchMap} from "rxjs";
import {UserDto, UsersClient} from "../../../web-api-client";
import {UntypedFormGroup} from "@angular/forms";

@Component({
  selector: 'app-admin-users-edit',
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header nzBackIcon (nzBack)="goBack()" [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Users</nz-breadcrumb-item>
            <nz-breadcrumb-item *ngIf="!isEdit">Add</nz-breadcrumb-item>
            <nz-breadcrumb-item *ngIf="isEdit">Edit</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title *ngIf="!isEdit">Create User</nz-page-header-title>
          <nz-page-header-title *ngIf="isEdit">Edit User</nz-page-header-title>
        </nz-page-header>
      </nz-col>
    </nz-row>
    <nz-row *ngIf="!loading; else loadingView">
      <pre>{{ user | json }}</pre>
    </nz-row>
    <ng-template #loadingView>
      <app-spinner></app-spinner>
    </ng-template>
  `
})
export class AdminUsersEditComponent implements OnChanges {
  id: number;
  user: UserDto;
  loading = false;
  isEdit = false;
  form: UntypedFormGroup = new UntypedFormGroup({});

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _toastService: ToastService,
    private _userClient: UsersClient
  ) {
    this.loading = true;
    const user$ = this._route.params.pipe(
      switchMap(params => {
        if (params['id']) {
          // Returning the observable directly instead of subscribing
          this.isEdit = true;
          return this._userClient.getUser(params['id']);
        } else {
          // If no id parameter is present, return an observable of new UserDto()
          this.isEdit = false;
          return of<UserDto>(new UserDto());
        }
      })
    );

// Subscribe to user$ observable elsewhere in your code if needed
    user$.subscribe({
      next: result => {
        this.user = result
        this.loading = false;
      },
      error: error => {
        console.error(error)
        this.loading = false;
      }
    });

  }

  ngOnChanges(changes: SimpleChanges): void {
    const {user} = changes;
    if (user && user.currentValue) {

    }
  }

  goBack(){
    this._router.navigate(['..'], { relativeTo: this._route });
  }
}
