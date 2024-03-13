import {Component, OnChanges} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {MapsClient} from "../../../web-api-client";
import {of, switchMap} from "rxjs";

@Component({
  selector: 'app-admin-maps-edit',
  template:`Edit of maps`
})
export class AdminMapsEditComponent  {

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
  //  private _toastService: ToastService,
    private _mapsClient: MapsClient
  ) {
    var test$ = this._route.params.pipe(
      switchMap((params) => {
        return params['id'] ? this._mapsClient.getMap(params['id']) : of(null);
      })
    );
  }


}
