import {Component, OnDestroy, OnInit} from "@angular/core";
import {MapDto, MapsClient, UserDto} from "../../web-api-client";
import {of, Subject, switchMap} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../shared/services/toast.service";

@Component({
  selector: 'app-map-detail',
  styles: [`
    .text-center {
      text-align: center;
    }
  `],
  templateUrl: './map-detail.component.html',
})
export class MapDetailComponent implements OnDestroy, OnInit {
  map: MapDto;
  loading = false;

  constructor(
    private _route: ActivatedRoute,
    private _toastService: ToastService,
    private _router: Router, private _mapsClient: MapsClient) {
  }

  ngOnInit(): void {
    this.loading = true;
    const map$ = this._route.params.pipe(
      switchMap((params) => {
          if (params['id']) {
            return this._mapsClient.getMap(params['id']);
          } else {
            return of<MapDto>(new MapDto());
          }
        }
      )
    );

    map$.subscribe({
        next: result => {
          this.map = result;
          this.loading = false;
        },
        error: error => {
          console.error(error);
          this.loading = false;
        }
      })
  }

  checkTimeExist(){
    console.log('checkTimeExist');
  }
  uploadTime(){
    console.log('uploadTime');
  }

  private readonly destroy$ = new Subject<void>();

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
