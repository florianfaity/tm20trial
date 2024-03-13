import {Component} from "@angular/core";
import {EStateValidation, MapDto, MapsClient} from "../../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import { ToastService } from "src/app/shared/services/toast.service";

@Component({
  selector: 'app-admin-maps-list',
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Maps</nz-breadcrumb-item>
            <nz-breadcrumb-item>Validate</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>List of validate maps</nz-page-header-title>
        </nz-page-header>
      </nz-col>

      <nz-col nzSpan="24">
        <nz-card nzBorderless>
          <nz-table #nzTable [nzData]="maps" [nzPageSize]="10" [nzShowSizeChanger]="true" nzShowSizeChanger
                    [nzLoading]="loading">
            <thead>
            <tr>
              <th>Name</th>
              <th>Author</th>
              <th>Difficulty</th>
              <th>Points</th>
              <th>Checkpoints</th>
              <th>Finishers</th>
              <th></th>
            </tr>
            </thead>
            <tbody>
            <tr *ngFor="let map of nzTable.data">
              <td>{{ map.name }}</td>
              <td>{{ map.author }}</td>
              <td>{{ map.difficulty | difficulty }}</td>
              <td>{{ map.points }}</td>
              <td>{{ map.numberCheckpoint }}</td>
              <td>{{ map.numberFinisher }}</td>
              <td nzAlign="center">
                <nz-button-group nzSize="small">     <button
                  nz-button
                  nzType="link"
                  nz-tooltip
                  nzTooltipTitle="Download"
                >
                  <i nz-icon nzType="download" nzTheme="outline"></i>
                </button>
                  <button
                    nz-button
                    nzType="link"
                    nz-tooltip
                    nzTooltipTitle="Edit"
                    (click)="goToEdit(map.id)"
                  >
                    <i nz-icon nzType="edit" nzTheme="outline"></i>
                  </button>
                  <button
                    nz-button
                    nzType="link"
                    nz-tooltip
                    nzTooltipTitle="Delete"
                    (click)="showModalDelete(map.id)"
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

    <!-- Modal delete map -->
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
        <p>You are about to delete a map.</p>
      </ng-template>

      <ng-template #modalFooter>
        <button nz-button nzType="primary" nzDanger nzGhost (click)="handleCancel()">Cancel</button>
        <button nz-button nzType="primary" (click)="removeMap()">Confirm</button>
      </ng-template>
    </nz-modal>

  `
})
export class AdminMapsListComponent {
  maps: MapDto[] = [];
  loading = false;
  modalDeleteDisplay = false;
  idMap: number;

  constructor(private mapsClient: MapsClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService
  ) {
    this.loading = true;
    mapsClient.getMaps(EStateValidation.Validate).subscribe({
      next: result => {
        this.maps = result
        this.loading = false
      },
      error: error => {
        console.error(error)
        this.loading = false
      }
    });
  }


  showModalDelete(id: number): void {
    this.modalDeleteDisplay = true;
    this.idMap = id;
  }


  handleCancel(): void {
    this.modalDeleteDisplay = false;
  }

  removeMap() {
    this.loading = true;
    this.mapsClient.deleteMap(this.idMap).subscribe({
      next: result => {
        this.maps = this.maps.filter(m => m.id !== this.idMap)
        this._toastService.success("Map deleted")
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
    console.log(id);
    this._router.navigate([id, 'edit'], {relativeTo: this._route});
  }
}
