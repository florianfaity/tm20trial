
import {gridResponsiveMap, NzBreakpointService} from "ng-zorro-antd/core/services";
import {shareReplay, Subject, takeUntil} from "rxjs";
import {CurrentUserDto, EDifficulty, EStateValidation, MapDto, MapsClient} from "../../web-api-client";
import {ToastService} from "../../shared/services/toast.service";
import {Component, OnDestroy} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpErrorResponse} from "@angular/common/http";
import {AuthorizeService} from "../../../api-authorization/authorize.service";

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

  isMapper: boolean = false;


  constructor(private mapsClient: MapsClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService,
              private _nzBreakpoint: NzBreakpointService,
              private _authorizeService: AuthorizeService,
  ) {
    this.loading = true;
    mapsClient.getMaps(EStateValidation.Validate).subscribe({
      next: result => {
        this.maps = result;
        this.loading = false;
      },
      error: error => {
        console.error(error);
        this.loading = false;
      }
    });

    this._nzBreakpoint.subscribe(gridResponsiveMap)
      .pipe(takeUntil(this.destroy$))
      .subscribe((c) => {
        this.currentBreakpoint = c;
      });


    this._authorizeService.getUser().pipe(shareReplay()).subscribe({
      next: (user: CurrentUserDto) => {
        this.isMapper = user.roles.indexOf('Administrator') > -1 || user.roles.indexOf('Mapper') > -1;
      },
      error: (err: HttpErrorResponse) => {
        if (err.status == 401) {
          this.isMapper = false;
        }
      }
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
  ];

  goToDetailMap(id : number){
    console.log("Id Map clicked",id);
    this._router.navigate([id], {relativeTo: this._route});
  }

  private readonly destroy$ = new Subject<void>();

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }


  goToSuggestMap(){
      this._router.navigate(['suggest'], {relativeTo: this._route});
  }
}
