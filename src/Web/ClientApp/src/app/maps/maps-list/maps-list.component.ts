
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";
import {Subject, takeUntil} from "rxjs";
import {EDifficulty, EStateValidation, MapDto, MapsClient} from "../../web-api-client";
import {ToastService} from "../../shared/services/toast.service";
import {Component, OnDestroy} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-maps-list',
  styles:[`
  .text-center{
    text-align: center;
  }
  `],
  templateUrl: './maps-list.component.html',
})
export class MapsListComponent implements OnDestroy {
  maps: MapDto[] = [];
  loading = false;
  currentBreakpoint: string = '';
  enumDifficulty = EDifficulty;


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

  listMapsByDifficulty(difficulty: EDifficulty){
    return this.maps.filter(m => m.difficulty == difficulty).sort(s => s.points);
  }

  listOfDifficulty = [
    {title: "Easy", difficulty: EDifficulty.Easy},
    {title: "Intermediate", difficulty: EDifficulty.Intermediate},
    {title: "Advanced", difficulty: EDifficulty.Advanced},
    {title: "Hard", difficulty: EDifficulty.Hard},
    {title: "Expert", difficulty: EDifficulty.Expert},
    {title: "Insane", difficulty: EDifficulty.Insane},
  ]

  private readonly destroy$ = new Subject<void>();
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
