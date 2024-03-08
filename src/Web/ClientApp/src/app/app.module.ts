import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';

import { AppComponent } from './app.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TodoComponent } from './todo/todo.component';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import {NzDropDownModule} from "ng-zorro-antd/dropdown";
import {NzAvatarModule} from "ng-zorro-antd/avatar";
import {NzLayoutModule} from "ng-zorro-antd/layout";
import {NzGridModule} from "ng-zorro-antd/grid";
import {NzInputModule} from "ng-zorro-antd/input";
import {MapsComponent} from "./maps/maps.component";
import {NzIconModule} from "ng-zorro-antd/icon";
import {SharedModule} from "./shared/shared.module";
import {IsRoleGuard} from "../api-authorization/guards/is-role.guard";

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    CounterComponent,
    FetchDataComponent,
    TodoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {
        path: '',
        loadChildren: () => import('./trackmania/trackmania.module').then((m) => m.TrackmaniaModule),
      },
      {
        path: 'admin',
        canActivate: [IsRoleGuard],
        data: {role: 'admin'},
        loadChildren: () => import('./admin/admin.module').then((m) => m.AdminModule),
      },
    ]),
    BrowserAnimationsModule,
    ModalModule.forRoot(),
    NzDropDownModule,
    NzAvatarModule,
    NzLayoutModule,
    NzGridModule,
    NzInputModule,
    NzIconModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true},
    {provide: NZ_I18N, useValue: en_US}
  ],
  exports: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
