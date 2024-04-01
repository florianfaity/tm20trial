import {Component, OnDestroy} from '@angular/core';
import {EStateValidation, MapDto, MapsClient} from "../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../shared/services/toast.service";
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
})
export class MapsComponent implements OnDestroy {
  maps: MapDto[] = [];
  loading = false;
  currentBreakpoint: string = '';

  constructor(private mapsClient: MapsClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService,
              private _nzBreakpoint: NzBreakpointService
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

    this._nzBreakpoint.subscribe(gridResponsiveMap)
      .pipe(takeUntil(this.destroy$))
      .subscribe((c) => {
        this.currentBreakpoint = c;
      });
  }




  private readonly destroy$ = new Subject<void>();
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
