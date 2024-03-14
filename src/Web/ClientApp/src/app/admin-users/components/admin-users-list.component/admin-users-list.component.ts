import {Component, OnDestroy} from "@angular/core";
import {EStateValidation, MapsClient, UserDto, UsersClient} from "../../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../../shared/services/toast.service";
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-admin-users-list',
  template:`
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Users</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>List of users</nz-page-header-title>
          <nz-page-header-extra>
            <button nz-button nzType="primary" (click)="goToAdd()" [nzShape]="(currentBreakpoint == 'xs') ? 'circle' : null">
              <i nz-icon nzType="plus" nzTheme="outline"></i><span *ngIf="currentBreakpoint != 'xs'">Create a user</span>
            </button>
          </nz-page-header-extra>
        </nz-page-header>
      </nz-col>

      <nz-col nzSpan="24">
        <nz-card nzBorderless>
          <nz-table #nzTable [nzData]="users" [nzPageSize]="10" [nzShowSizeChanger]="true" nzShowSizeChanger
                    [nzLoading]="loading">
            <thead>
            <tr>
              <th>id</th>
              <th>Display name</th>
              <th>Login Uplay</th>
              <th>Nation</th>
              <th>Twitch </th>
              <th>Twitter </th>
              <th>Tm.io Id </th>
              <th></th>
            </tr>
            </thead>
            <tbody>
            <tr *ngFor="let user of nzTable.data">
              <td>{{ user.idUser }}</td>
              <td>{{ user.displayName }}</td>
              <td>{{ user.loginUplay }}</td>
              <td>{{ user.nation }}</td>
              <td>{{ user.twitchUsername }}</td>
              <td>{{ user.twitterUsername }}</td>
              <td>{{ user.tmIoId }}</td>
              <td nzAlign="center">
                <nz-button-group nzSize="small">

                  <button
                    nz-button
                    nzType="link"
                    nz-tooltip
                    nzTooltipTitle="Edit"
                    (click)="goToEdit(user.idUser)"
                  >
                    <i nz-icon nzType="edit" nzTheme="outline"></i>
                  </button>
                  <button
                    nz-button
                    nzType="link"
                    nz-tooltip
                    nzTooltipTitle="Delete"
                    (click)="showModalDelete(user.idUser)"
                  >
                    <i nz-icon nzType="delete" nzTheme="outline" class="icon-error"></i>
                  </button>
                </nz-button-group>
              </td>
            </tr>
            </tbody>
          </nz-table>
        </nz-card>
      </nz-col>
    </nz-row>

    <!-- Modal delete user -->
    <nz-modal
      [(nzVisible)]="modalDeleteDisplay"
      nzTitle="Delete Map"
      (nzOnCancel)="handleCancel()"
      [nzTitle]="modalTitle"
      [nzContent]="modalContent"
      [nzFooter]="modalFooter"
    >
      <ng-template #modalTitle>
        <i nz-icon nzType="warning" nzTheme="twotone" [nzTwotoneColor]="'#fa625b'"></i>
        Warning
      </ng-template>

      <ng-template #modalContent>
        <p>You are about to delete a user.</p>
      </ng-template>

      <ng-template #modalFooter>
        <button nz-button nzType="primary" nzDanger nzGhost (click)="handleCancel()">Cancel</button>
        <button nz-button nzType="primary" (click)="removeUser()">Confirm</button>
      </ng-template>
    </nz-modal>
  `
})
export class AdminUsersListComponent implements OnDestroy{
  modalDeleteDisplay = false;
  loading = false;
  users: UserDto[] = [];
  idUser:number
  currentBreakpoint: string = '';

  constructor(private usersClient: UsersClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService,
              private _nzBreakpoint: NzBreakpointService
  ) {
    this.loading = true;
    usersClient.getUsers().subscribe({
      next: result => {
        this.users = result
        this.loading = false
      },
      error: error => {
        console.error(error)
        this.loading = false
      }
    });
    this._nzBreakpoint.subscribe(gridResponsiveMap)
      .pipe(takeUntil(this.destroy$))
      .subscribe((c) => {
        this.currentBreakpoint = c;
      });
  }


  showModalDelete(id: number): void {
    this.modalDeleteDisplay = true;
    this.idUser = id;
  }


  handleCancel(): void {
    this.modalDeleteDisplay = false;
  }

  removeUser() {
    this.loading = true;
    this.usersClient.deleteUser(this.idUser).subscribe({
      next: result => {
        this.users = this.users.filter(m => m.idUser !== this.idUser)
        this._toastService.success("User deleted")
        this.loading = false
      },
      error: error => {
        console.error(error)
        this._toastService.error("Error")
        this.loading = false
      }
    });
    this.modalDeleteDisplay = false;
  }

  goToEdit(id: number) {
    this._router.navigate([id, 'edit'], {relativeTo: this._route});
  }

  goToAdd() {
    this._router.navigate(['add'], {relativeTo: this._route});
  }


  private readonly destroy$ = new Subject<void>();
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
