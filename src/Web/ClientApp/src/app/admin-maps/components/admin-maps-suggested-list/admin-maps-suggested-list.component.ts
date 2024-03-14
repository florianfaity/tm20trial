import {Component} from "@angular/core";
import {EStateValidation, MapDto, MapsClient} from "../../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-admin-maps-suggested-list',
  template:`

    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Maps</nz-breadcrumb-item>
            <nz-breadcrumb-item>Suggested</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>List of suggested maps </nz-page-header-title>
        </nz-page-header>
      </nz-col>

      <nz-col nzSpan="24">
        <nz-card nzBorderless>
          <nz-table #nzTable [nzData]="maps" [nzPageSize]="10" [nzShowSizeChanger]="true" nzShowSizeChanger [nzLoading]="loading">
            <thead >
            <tr>
              <th>Name</th>
              <th>Author</th>
              <th>Difficulty</th>
              <th>Points</th>
              <th></th>
            </tr>
            </thead>
            <tbody >
            <tr  *ngFor="let map of nzTable.data">
              <td>{{map.name}}</td>
              <td>{{map.author}}</td>
              <td>{{map.difficulty | difficulty}}</td>
              <td>{{map.points}}</td>
              <td nzAlign="center">
                <nz-button-group nzSize="small">
                  <button nz-button nzType="link" nz-tooltip nzTooltipTitle="Validate"
                          (click)="changeStateMap(enumStateValidation.Validate, map.id)">

                    <span nz-icon [nzType]="'check-circle'" [nzTheme]="'twotone'" [nzTwotoneColor]="'#52c41a'"></span>

                  </button>
                  <button
                    nz-button
                    nzType="link"
                    nz-tooltip
                    nzTooltipTitle="Refuse"
                    (click)="changeStateMap(enumStateValidation.Refuse, map.id)">
                    <span nz-icon nzType="close-circle" nzTheme="outline"></span>
<!--                    <span nz-icon [nzType]="'check-close'" [nzTheme]="'twotone'"></span>-->
                    <!--                    <i nz-icon nzType="Close" nzTheme="outline"></i>-->
                  </button>
                  <button
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
export class AdminMapsSuggestedListComponent{
  maps: MapDto[] = [];
  loading = false;
  modalDeleteDisplay = false;
  idMap:number;
  enumStateValidation = EStateValidation;
  constructor(private mapsClient: MapsClient,
  private _route: ActivatedRoute,
  private _router: Router ) {
    this.loading = true;
    mapsClient.getMaps(EStateValidation.New).subscribe({
      next: result => {
        this.maps = result
        this.loading = false
      },
      error: error => {
        console.error(error)
        this.loading = false}
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
        this.loading = false
      },
      error: error => {
        console.error(error)
        this.loading = false
      }
    });
    this.modalDeleteDisplay = false;
  }

  changeStateMap(state: EStateValidation, id: number) {
    this.loading = true;
    this.mapsClient.updateStateMap(id, state).subscribe({
      next: result => {
        this.maps = this.maps.filter(m => m.id !== id)
        this.loading = false
      },
      error: error => {
        console.error(error)
        this.loading = false
      }
    });
    this.modalDeleteDisplay = false;
  }


  goToEdit(id: number) {
    this._router.navigate([id, 'edit'], {relativeTo: this._route});
  }

}
