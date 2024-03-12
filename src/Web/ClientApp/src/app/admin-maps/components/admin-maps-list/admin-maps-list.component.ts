import {Component} from "@angular/core";
import {EStateValidation, MapDto, MapsClient} from "../../../web-api-client";

@Component({
  selector: 'app-admin-maps-list',
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header>

          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Maps</nz-breadcrumb-item>
            <nz-breadcrumb-item>List</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title>List of maps validate</nz-page-header-title>
        </nz-page-header>
      </nz-col>

      <nz-col nzSpan="24">
        <nz-card nzBorderless>
              <nz-table #nzTable [nzData]="maps" [nzPageSize]="10" [nzShowSizeChanger]="true" [nzShowQuickJumper]="true">
                <thead >
                <tr>
                  <th>Name</th>
                  <th>Author</th>
                  <th>Difficulty</th>
                  <th>Points</th>
                  <th>Checkpoints</th>
                  <th>Finishers</th>
                </tr>
                </thead>
                <tbody >
                <tr  *ngFor="let data of nzTable.data">
                  <td>{{data.name}}</td>
                  <td>{{data.author}}</td>
                  <td>{{data.difficulty}}</td>
                  <td>{{data.points}}</td>
                  <td>{{data.numberCheckpoint}}</td>
                  <td>{{data.numberFinisher}}</td>
                  <td>
                    <a nz-tooltip nzTitle="Edit" nzPlacement="top"><i nz-icon nzType="edit" nzTheme="outline"></i></a>
                    <a nz-tooltip nzTitle="Delete" nzPlacement="top"><i nz-icon nzType="delete" nzTheme="outline"></i></a>
                  </td>
                </tr>
                </tbody>
              </nz-table>
        </nz-card>
      </nz-col>
    </nz-row>
  `
})
export class AdminMapsListComponent {
  maps: MapDto[] = [];
  constructor(private mapsClient: MapsClient ) {
    mapsClient.getMaps(EStateValidation.Validate).subscribe({
      next: result => this.maps = result,
      error: error => console.error(error)
    });
  }
}
