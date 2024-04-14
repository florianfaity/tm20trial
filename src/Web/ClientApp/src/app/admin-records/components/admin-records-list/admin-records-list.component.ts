import {Component, OnDestroy} from "@angular/core";
import {Subject, takeUntil} from "rxjs";
import {EStateValidation, MapsClient, RecordDto, RecordsClient} from "../../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../../shared/services/toast.service";
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";

@Component({
  selector: 'app-admin-maps-list',
  styles: [
    `
      .search-box {
        padding: 8px;
      }

      .search-box input {
        width: 188px;
        margin-bottom: 8px;
        display: block;
      }

      .search-box button {
        width: 90px;
      }

      .search-button {
        margin-right: 8px;
      }
    `
  ],
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Records</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>List of records</nz-page-header-title>
        </nz-page-header>
      </nz-col>

      <nz-col nzSpan="24">
        <nz-card nzBorderless>
          <nz-table #nzTable [nzData]="listOfDataDisplay" [nzPageSize]="10" [nzShowSizeChanger]="true" nzShowSizeChanger
                    [nzLoading]="loading" nzSize="small">
            <thead>
            <tr>
              <th>Id</th>
              <th>Map name
                <nz-filter-trigger [(nzVisible)]="visibleMapName" [nzDropdownMenu]="menuMap">
                  <span nz-icon nzType="search"></span>
                </nz-filter-trigger>
              </th>
              <th>User
                <nz-filter-trigger [(nzVisible)]="visibleUserName" [nzDropdownMenu]="menuUser">
                  <span nz-icon nzType="search"></span>
                </nz-filter-trigger>
              </th>
              <th nzAlign="center">Validate</th>
              <th>Time</th>
              <th>Date PB set</th>
              <th  nzAlign="center">Action</th>
            </tr>
            </thead>
            <tbody>
            <tr *ngFor="let record of nzTable.data">
              <td>{{ record.id }}</td>
              <td>{{ record.mapName }}</td>
              <td>{{ record.displayName }}</td>
              <td nzAlign="center">
                <span *ngIf="record.isValidated" nz-icon [nzType]="'check-circle'" [nzTheme]="'twotone'"
                      [nzTwotoneColor]="'#52c41a'"></span>

                <span *ngIf="!record.isValidated" nz-icon nzType="close-circle" nzTheme="outline"></span>
              </td>
              <td>{{ record.time }}</td>
              <td>{{ record.datePersonalBest | date: 'dd/MM/yyyy' }} {{ record.datePersonalBest | date: 'shortTime' }}</td>
              <td nzAlign="center">
                <button *ngIf="!record.isValidated" nz-button nzType="link" nz-tooltip nzTooltipTitle="Validate"
                        (click)="validateRecord(record.id, true)">
                <span nz-icon [nzType]="'check-circle'" [nzTheme]="'twotone'"
                      [nzTwotoneColor]="'#52c41a'"></span>
                </button>
                <button *ngIf="record.isValidated"
                  nz-button
                  nzType="link"
                  nz-tooltip
                  nzTooltipTitle="Refuse record"
                  (click)="validateRecord(record.id, false)">
                    <span nz-icon nzType="close-circle" nzTheme="outline"></span>
                </button>
                <a
                  nz-button
                  nzType="link"
                  nz-tooltip
                  nzTooltipTitle="Download"
                  [href]="record.fileUrl.replace('https://core.trackmania.nadeo.live/storageObjects','https://trackmania.io/api/download/ghost')"
                >
                  <i nz-icon nzType="download" nzTheme="outline"></i>
                </a>
              </td>
            </tr>
            </tbody>
          </nz-table>
        </nz-card>
      </nz-col>
    </nz-row>
    <nz-dropdown-menu #menuMap="nzDropdownMenu">
      <div class="ant-table-filter-dropdown">
        <div class="search-box">
          <input type="text" nz-input placeholder="Search Map Name" [(ngModel)]="searchMapNameValue"/>
          <button nz-button nzSize="small" nzType="primary" (click)="searchMapName()" class="search-button">Search
          </button>
          <button nz-button nzSize="small" (click)="resetMapName()">Reset</button>
        </div>
      </div>
    </nz-dropdown-menu>
    <nz-dropdown-menu #menuUser="nzDropdownMenu">
      <div class="ant-table-filter-dropdown">
        <div class="search-box">
          <input type="text" nz-input placeholder="Search User Name" [(ngModel)]="searchUserNameValue"/>
          <button nz-button nzSize="small" nzType="primary" (click)="searchUserName()" class="search-button">Search
          </button>
          <button nz-button nzSize="small" (click)="resetUserName()">Reset</button>
        </div>
      </div>
    </nz-dropdown-menu>
  `
})
export class AdminRecordsListComponent implements OnDestroy {
  records: RecordDto[];
  loading = false;
  currentBreakpoint: string = '';
  visibleMapName = false;
  visibleUserName = false;
  searchMapNameValue = '';
  searchUserNameValue = '';
  listOfDataDisplay: RecordDto[] = [];

  constructor(private recordsClient: RecordsClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService,
              private _nzBreakpoint: NzBreakpointService
  ) {
    this.loadData();

    this._nzBreakpoint.subscribe(gridResponsiveMap)
      .pipe(takeUntil(this.destroy$))
      .subscribe((c) => {
        this.currentBreakpoint = c;
      });
  }

  loadData() {
    this.loading = true;
    this.recordsClient.getRecords().subscribe({
      next: result => {
        this.records = result
        this.listOfDataDisplay = result
        this.loading = false
      },
      error: error => {
        console.error(error)
        this.loading = false
      }
    });

  }

  resetMapName(): void {
    this.searchMapNameValue = '';
    this.searchMapName();
  }

  resetUserName(): void {
    this.searchUserNameValue = '';
    this.searchUserName();
  }

  searchMapName(): void {
    this.visibleMapName = false;
    this.listOfDataDisplay = this.records.filter((item: RecordDto) => item.mapName.indexOf(this.searchMapNameValue) !== -1);
  }

  searchUserName(): void {
    this.visibleUserName = false;
    this.listOfDataDisplay = this.records.filter((item: RecordDto) => item.displayName.indexOf(this.searchUserNameValue) !== -1);
  }

  validateRecord(id: number, validate: boolean) {
    this.recordsClient.updateStateRecord(id, validate).subscribe({
      next: result => {
        this._toastService.success('Record updated successfully');
        this.loadData();
      },
      error: error => {
        console.error(error)
      }
    })

  }

  private readonly destroy$ = new Subject<void>();

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  protected readonly enumStateValidation = EStateValidation;
}
