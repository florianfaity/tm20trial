import {Component, OnDestroy} from "@angular/core";
import {Subject, takeUntil} from "rxjs";
import {EStateValidation, MapDto, MapsClient} from "../../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../../shared/services/toast.service";
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";

@Component({
  selector: 'app-admin-maps-list',
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Maps</nz-breadcrumb-item>
            <nz-breadcrumb-item>Refused</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>List of refused maps</nz-page-header-title>
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
                <ng-template #imagePreviewTemplate>
                  <img
                    nz-image
                    width="400px;" height="400px;"
                    *ngIf="map.imageLink"
                    [nzSrc]="map.imageLink"
                    alt=""
                  />
                </ng-template>
                <nz-button-group nzSize="small">
                  <button nz-button nzType="link" nz-tooltip nzTooltipTitle="Validate"
                          (click)="changeStateMap(enumStateValidation.Validate, map.id)">

                    <span nz-icon [nzType]="'check-circle'" [nzTheme]="'twotone'" [nzTwotoneColor]="'#52c41a'"></span>

                  </button>
                  <button nz-button nzType="link" nz-popover
                          [nzPopoverContent]="imagePreviewTemplate" nzPopoverTrigger="hover">
                    <i nz-icon nzType="file-image" nzTheme="outline"></i>
                  </button>
                  <a
                  nz-button
                  nzType="link"
                  nz-tooltip
                  nzTooltipTitle="Download"
                  [href]="map.fileUrl" target="_blank"
                >
                  <i nz-icon nzType="download" nzTheme="outline"></i>
                </a>
                  <button
                    nz-button
                    nzType="link"
                    nz-tooltip
                    nzTooltipTitle="Edit"
                    (click)="goToEdit(map.id)"
                  >
                    <i nz-icon nzType="edit" nzTheme="outline"></i>
                  </button>
                </nz-button-group>
              </td>
            </tr>
            </tbody>
          </nz-table>
        </nz-card>
      </nz-col>
    </nz-row>
  `
})
export class AdminMapsRefusedListComponent implements OnDestroy {
  maps: MapDto[] = [];
  loading = false;
  modalDeleteDisplay = false;
  idMap: number;
  currentBreakpoint: string = '';

  constructor(private mapsClient: MapsClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService,
              private _nzBreakpoint: NzBreakpointService
  ) {
    this.loading = true;
    mapsClient.getMaps(EStateValidation.Refuse).subscribe({
      next: result => {
        this.maps = result
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
    this._router.navigate(['..',id, 'edit'], {relativeTo: this._route});
  }

  private readonly destroy$ = new Subject<void>();
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  protected readonly enumStateValidation = EStateValidation;
}
