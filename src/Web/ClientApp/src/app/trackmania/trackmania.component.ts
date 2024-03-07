import {Component, OnDestroy, OnInit} from "@angular/core";

@Component({
  selector: 'app-trackmania',
  template: `
<app-nav-menu [isAdminView]="false" >      <router-outlet></router-outlet></app-nav-menu>

  `,
})
export class TrackmaniaComponent implements OnInit, OnDestroy {
    ngOnDestroy(): void {

    }
    ngOnInit(): void {

    }
}
