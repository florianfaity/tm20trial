import {Component, OnDestroy, OnInit} from "@angular/core";
import {MapDto, MapsClient, RecordsClient, UserDto} from "../../web-api-client";
import {of, Subject, switchMap} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../shared/services/toast.service";
import {DomSanitizer, SafeResourceUrl} from "@angular/platform-browser";

@Component({
  selector: 'app-map-detail',
  templateUrl: './map-detail.component.html',
})
export class MapDetailComponent implements OnDestroy, OnInit {
  map: MapDto;
  loading = false;
  safeURL:SafeResourceUrl;

  constructor(
    private _route: ActivatedRoute,
    private _recordsService: RecordsClient,
    private _toastService: ToastService,
    private _router: Router, private _mapsClient: MapsClient, private _sanitizer: DomSanitizer) {
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
          if(this.map.videoLink != null)
            this.safeURL = this._sanitizer.bypassSecurityTrustResourceUrl(this.map.videoLink.replace("https://www.youtube.com/watch?v=", "https://www.youtube.com/embed/"));
          else
            this.safeURL = null;
          this.loading = false;
        },
        error: error => {
          console.error(error);
          this.loading = false;
        }
      })
  }

  checkTimeExist(){
    console.log(this.map.id);
    if(this.map && this.map.id){
      this._recordsService.updateRecordByIoId(this.map.id).subscribe({
        next: result => {
          this._toastService.success("Record updated")
         // this._router.navigate(['..'], {relativeTo: this._route});
        },
        error: error => {
          console.error(error)
          this._toastService.error("Error")
        }
      });
    }
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
