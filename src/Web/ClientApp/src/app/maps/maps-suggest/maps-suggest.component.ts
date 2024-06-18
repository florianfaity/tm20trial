import {Component, OnDestroy} from "@angular/core";
import {
  CreateMapCommand,
  EDifficulty,
  EStateValidation,
  ETypeTrial,
  MapsClient,
  OpenplanetClient
} from "../../web-api-client";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastService} from "../../shared/services/toast.service";
import {NzBreakpointService} from "ng-zorro-antd/core/services";
import {AuthorizeService} from "../../../api-authorization/authorize.service";
import {UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {NzImageService} from "ng-zorro-antd/image";

@Component({
  selector: 'app-maps-suggest',
  styles:[ `
    .margin-line {
      margin-top: 5px;
      margin-bottom: 10px;
    }
    .text-center{
      text-align: center;
    }
  `,],
  templateUrl: `maps-suggest.component.html`
})
export class MapsSuggestComponent {
  enumDifficulty = EDifficulty;
  enumTypeTrial = ETypeTrial;
  enumStateValidation = EStateValidation;
  form: UntypedFormGroup = new UntypedFormGroup({});
  srcImage = '';
  idMap = '';

  constructor(private _mapsClient: MapsClient,
              private _route: ActivatedRoute,
              private _router: Router,
              private _toastService: ToastService,
              private _nzBreakpoint: NzBreakpointService,
              private _authorizeService: AuthorizeService,
              private _openplanetService: OpenplanetClient,
              private nzImageService: NzImageService,
              fb: UntypedFormBuilder
  ) {

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


  setTmIoId(id: any) {
    this.idMap = id.value;
  }

  clickTmIoId() {
    this._openplanetService.searchNadeoMap(this.idMap).subscribe(data => {
        console.log(data);
        this.form.get('name').setValue(data.name);
        this.form.get('author').setValue(data.authorDisplayName);
        this.form.get('imageLink').setValue(data.thumbnailUrl);
        this.srcImage = data.thumbnailUrl;
        this.form.get('fileUrl').setValue(data.fileUrl);
      }
    );
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
  onSubmit(){
    console.log("this.form.value");
    console.log(this.form.value);
    const modelToSubmit = new CreateMapCommand({
      ...this.form.value,
    })
    this._mapsClient.createMap(modelToSubmit).subscribe({
      next: result => {
        this._toastService.success("Map suggested")
        this.goBack();
      },
      error: error => {
        console.error(error)
        this._toastService.error("Error")
      }
    });

  }
  goBack() {
      this._router.navigate(['..'], {relativeTo: this._route});
  }
}
