import {Component, OnChanges, OnInit, SimpleChanges} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {
  CreateMapCommand,
  EDifficulty,
  EStateValidation,
  ETypeTrial,
  MapDto,
  MapsClient, OpenplanetClient,
  UpdateMapCommand
} from "../../../web-api-client";
import {of, switchMap} from "rxjs";
import {ToastService} from "../../../shared/services/toast.service";
import {UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import { NzImageService } from "ng-zorro-antd/image";
import {MapsService} from "../../../shared/services/maps.service";

@Component({
  selector: 'app-admin-maps-edit',
  styles: [
    `
      .margin-line {
        margin-top: 5px;
        margin-bottom: 10px;
      }
    `,
  ],
  template: `
    <nz-row [nzGutter]="[16, 16]">
      <nz-col nzSpan="24">
        <nz-page-header nzBackIcon (nzBack)="goBack()" [nzGhost]="false">
          <nz-breadcrumb nz-page-header-breadcrumb>
            <nz-breadcrumb-item>Maps</nz-breadcrumb-item>
            <nz-breadcrumb-item *ngIf="!isEdit">Add</nz-breadcrumb-item>
            <nz-breadcrumb-item *ngIf="isEdit && map != null && map.state == enumStateValidation.Validate" (click)="goBack()" class="cursor-pointer">List of validate map </nz-breadcrumb-item>
            <nz-breadcrumb-item *ngIf="isEdit && map != null && map.state == enumStateValidation.New" (click)="goBack()" class="cursor-pointer">List of suggested map </nz-breadcrumb-item>
            <nz-breadcrumb-item *ngIf="isEdit">Edit</nz-breadcrumb-item>
          </nz-breadcrumb>
          <nz-page-header-title *ngIf="!isEdit">Create Map</nz-page-header-title>
          <nz-page-header-title *ngIf="isEdit">Edit Map</nz-page-header-title>
        </nz-page-header>
      </nz-col>


      <nz-col nzSpan="24" *ngIf="!loading; else loadingView">
        <nz-card nzBorderless>
          <form [formGroup]="form">
            <nz-row>
              <nz-col class="margin-line" [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row>
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzRequired nzFor="tmIoId">
                    Trackmania.io Id
                  </nz-form-label>
                  <nz-form-control nz-col [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                    <nz-input-group nzSearch [nzAddOnAfter]="suffixButton">
                      <input type="text" nz-input formControlName="tmIoId" (blur)="setTmIoId($event.target)"/>
                    </nz-input-group>
                    <ng-template #suffixButton>
                      <button nz-button nzType="primary" (click)="clickTmIoId()" nzSearch>Search</button>
                    </ng-template>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzRequired nzFor="name">
                    Name
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                    <input nz-input formControlName="name"/>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzRequired nzFor="author">
                    Author
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                    <input nz-input formControlName="author"/>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzFor="difficulty">
                    Difficulty
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24">
                    <nz-select formControlName="difficulty">
                      <nz-option [nzValue]="enumDifficulty.Easy" nzLabel="Easy"></nz-option>
                      <nz-option [nzValue]="enumDifficulty.Intermediate" nzLabel="Intermediate"></nz-option>
                      <nz-option [nzValue]="enumDifficulty.Advanced" nzLabel="Advanced"></nz-option>
                      <nz-option [nzValue]="enumDifficulty.Hard" nzLabel="Hard"></nz-option>
                      <nz-option [nzValue]="enumDifficulty.Expert" nzLabel="Expert"></nz-option>
                      <nz-option [nzValue]="enumDifficulty.Insane" nzLabel="Insane"></nz-option>
                    </nz-select>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzFor="typeTrial">
                    Type trial
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24">
                    <nz-select formControlName="typeTrial">
                      <nz-option [nzValue]="enumTypeTrial.Classic" nzLabel="Classic"></nz-option>
                      <nz-option [nzValue]="enumTypeTrial.Fun" nzLabel="Fun"></nz-option>
                      <nz-option [nzValue]="enumTypeTrial.Mini" nzLabel="Mini"></nz-option>
                    </nz-select>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzRequired nzFor="points">
                    Points
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                    <nz-input-number formControlName="points"
                                     [nzMin]="1" [nzMax]="1500" [nzStep]="1"></nz-input-number>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzFor="tmxLink">
                    Trackmania exchange link
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24">
                    <input nz-input formControlName="tmxLink"/>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzFor="videoLink">
                    Vidéo link or GPS
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24">
                    <input nz-input formControlName="videoLink"/>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzFor="imageLink">
                    Image link
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24">
                    <nz-input-group nzSearch [nzAddOnAfter]="previewButton">
                      <input nz-input formControlName="imageLink" (blur)="setImage($event.target)"/>
                    </nz-input-group>
                    <ng-template #previewButton>
                      <button nz-button nzType="primary" (click)="previewImage()" nzSearch>Preview</button>
                    </ng-template>
                  </nz-form-control>
                </nz-row>
              </nz-col>
              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzRequired nzFor="numberCheckpoint">
                    Number of checkpoints
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24" nzErrorTip="Required">
                    <nz-input-number formControlName="numberCheckpoint"
                                     [nzMin]="0" [nzMax]="1500" [nzStep]="1"></nz-input-number>
                  </nz-form-control>
                </nz-row>
              </nz-col>

              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-form-label class="text-left" [nzLg]="8" [nzXs]="24" nzFor="state">
                    State of the map
                  </nz-form-label>
                  <nz-form-control [nzLg]="16" [nzXs]="24">
                    <nz-select formControlName="state">
                      <nz-option [nzValue]="enumStateValidation.New" nzLabel="New"></nz-option>
                      <nz-option [nzValue]="enumStateValidation.InProgress" nzLabel="In progress"></nz-option>
                      <nz-option [nzValue]="enumStateValidation.Refuse" nzLabel="Refuse"></nz-option>
                      <nz-option [nzValue]="enumStateValidation.Validate" nzLabel="Validate"></nz-option>
                    </nz-select>
                  </nz-form-control>
                </nz-row>
              </nz-col>

              <nz-col [nzLg]="{ span: 12, offset: 6 }" [nzXs]="24" style="margin-top: 5px;">
                <nz-row class="margin-line" nzJustify="space-between">
                  <nz-col>
                    <button nz-button nzType="primary" nzGhost class="margin-button" type="button"
                            (click)="goBack()">
                      <i nz-icon nzType="close" nzTheme="outline"></i> Annuler
                    </button>
                  </nz-col>
                  <nz-col>
                    <button nz-button nzType="primary" class="margin-button-edition" (click)="onSubmit()">
                      <i nz-icon nzType="save" nzTheme="outline"></i>
                      {{ isEdit ? 'Update' : 'Add' }}
                    </button>
                  </nz-col>
                </nz-row>
              </nz-col>
            </nz-row>
          </form>
        </nz-card>
      </nz-col>
    </nz-row>
    <ng-template #loadingView>
      <app-spinner></app-spinner>
    </ng-template>
  `
})
export class AdminMapsEditComponent implements OnInit, OnChanges {
  map: MapDto;
  loading = false;
  isEdit = false;
  enumDifficulty = EDifficulty;
  enumTypeTrial = ETypeTrial;
  enumStateValidation = EStateValidation;
  form: UntypedFormGroup = new UntypedFormGroup({});
  srcImage = '';
  idMap = '';

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _toastService: ToastService,
    private _mapsClient: MapsClient, private nzImageService: NzImageService,
    private _openplanetService: OpenplanetClient,
    fb: UntypedFormBuilder
  ) {
    this.loading = true;
    const map$ = this._route.params.pipe(
      switchMap(params => {
        if (params['id']) {
          // Returning the observable directly instead of subscribing
          this.isEdit = true;
          return this._mapsClient.getMap(params['id']);
        } else {
          // If no id parameter is present, return an observable of new UserDto()
          this.isEdit = false;
          return of<MapDto>(new MapDto());
        }
      })
    );

// Subscribe to user$ observable elsewhere in your code if needed
    map$.subscribe({
      next: result => {
        this.map = result
        this.loading = false;
        this.setValue();
      },
      error: error => {
        console.error(error)
        this.loading = false;
      }
    });

    this.form = fb.group({
      name: [null, Validators.required],
      author: [null, Validators.required],
      difficulty: [null, Validators.required],
      typeTrial: [ETypeTrial.Classic, Validators.required],
      points: [1],
      tmxLink: [],
      videoLink: [],
      imageLink: [],
      fileUrl: [],
      numberCheckpoint: [0, Validators.required],
      tmIoId: [null, Validators.required],
      state: [EStateValidation.New, Validators.required],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
    console.log(changes.map);
    }

  ngOnInit(): void {
    this.setValue();
  }

  setValue(){
    if (this.isEdit && this.map?.id != null) {
      this.form.get('name').setValue(this.map.name);
      this.form.get('author').setValue(this.map.author);
      this.form.get('difficulty').setValue(this.map.difficulty);
      this.form.get('typeTrial').setValue(this.map.typeTrial);
      this.form.get('points').setValue(this.map.points);
      this.form.get('tmxLink').setValue(this.map.tmxLink);
      this.form.get('videoLink').setValue(this.map.videoLink);
      this.form.get('imageLink').setValue(this.map.imageLink);
      this.form.get('numberCheckpoint').setValue(this.map.numberCheckpoint);
      this.form.get('tmIoId').setValue(this.map.tmIoId);
      this.form.get('state').setValue(this.map.state);
      this.form.get('fileUrl').setValue(this.map.fileUrl);
      this.srcImage = this.map.imageLink;
      this.idMap = this.map.tmIoId;
    }
  }

  onSubmit() {
    if (!this.form.valid) {
      return;
    }
    if (this.isEdit) {
      const modelToSubmit = new UpdateMapCommand({
        ...this.form.value,
      })
      this._mapsClient.updateMap(this.map.id, modelToSubmit).subscribe({
        next: result => {
          this._toastService.success("Map updated")
          this._router.navigate(['..'], {relativeTo: this._route});
        },
        error: error => {
          console.error(error)
          this._toastService.error("Error")
        }
      });
    } else {
      const modelToSubmit = new CreateMapCommand({
        ...this.form.value,
      })
      this._mapsClient.createMap(modelToSubmit).subscribe({
        next: result => {
          this._toastService.success("Map created")
          this._router.navigate(['..'], {relativeTo: this._route});
        },
        error: error => {
          console.error(error)
          this._toastService.error("Error")
        }
      });

    }
  }

  setTmIoId(id: any) {
    this.idMap = id.value;
  }


  clickTmIoId() {
    console.log(this.idMap);
    //TODO API OPENPLANET AUTO COMPLETE TRACKMANIA.IO
    this._openplanetService.searchNadeoMap(this.idMap).subscribe(data => {
      console.log(data);
      this.form.get('name').setValue(data.name);
      this.form.get('author').setValue(data.authorDisplayName);

      this.form.get('imageLink').setValue(data.thumbnailUrl);
      this.srcImage = data.thumbnailUrl;
      this.form.get('fileUrl').setValue(data.fileUrl);
      // this.form.get('tmxLink').setValue(this.map.tmxLink);
      // this.form.get('videoLink').setValue(this.map.videoLink);
      // this.form.get('numberCheckpoint').setValue(this.map.numberCheckpoint);
      // this.form.get('tmIoId').setValue(this.map.tmIoId);
      // this.form.get('state').setValue(this.map.state);
      //
     //  this.idMap = this.map.tmIoId;

      }
    );

  }


  goBack() {
    if (this.map.state == EStateValidation.Validate)
      this._router.navigate(['../..'], {relativeTo: this._route});
    else if (this.map.state == EStateValidation.New)
      this._router.navigate(['Maps', 'Suggested'], {relativeTo: this._route});
    else if (this.map.state == EStateValidation.Refuse)
      this._router.navigate(['Maps', 'Refused'], {relativeTo: this._route});
    else
      this._router.navigate(['..'], {relativeTo: this._route});
  }

  setImage(imageLink: any) {
    this.srcImage = imageLink.value;
  }

  previewImage(): void {
    const images = [
      {
        src: this.srcImage,
        alt: ''
      }
    ];
    this.nzImageService.preview(images, { nzZoom: 1, nzRotate: 0 });
  }

}
