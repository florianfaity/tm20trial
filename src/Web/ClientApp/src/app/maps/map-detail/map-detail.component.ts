import {Component, OnDestroy} from "@angular/core";
import {MapDto} from "../../web-api-client";
import {Subject} from "rxjs";

@Component({
  selector: 'app-map-detail',
  styles:[`
  .text-center{
    text-align: center;
  }
  `],
  templateUrl: './map-detail.component.html',
})
export class MapDetailComponent implements OnDestroy {
  maps: MapDto;
  loading = false;



  private readonly destroy$ = new Subject<void>();
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
