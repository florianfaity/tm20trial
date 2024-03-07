import {NzAffixModule} from 'ng-zorro-antd/affix';
import {NzAlertModule} from 'ng-zorro-antd/alert';
import {NzAnchorModule} from 'ng-zorro-antd/anchor';
import {NzAutocompleteModule} from 'ng-zorro-antd/auto-complete';
import {NzAvatarModule} from 'ng-zorro-antd/avatar';
import {NzBadgeModule} from 'ng-zorro-antd/badge';
import {NzBreadCrumbModule} from 'ng-zorro-antd/breadcrumb';
import {NzButtonModule} from 'ng-zorro-antd/button';
import {NzCalendarModule} from 'ng-zorro-antd/calendar';
import {NzCardModule} from 'ng-zorro-antd/card';
import {NzCarouselModule} from 'ng-zorro-antd/carousel';
import {NzCheckboxModule} from 'ng-zorro-antd/checkbox';
import {NzCollapseModule} from 'ng-zorro-antd/collapse';
import {NzDatePickerModule} from 'ng-zorro-antd/date-picker';
import {NzDescriptionsModule} from 'ng-zorro-antd/descriptions';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import {NzDividerModule} from 'ng-zorro-antd/divider';
import {NzDrawerModule} from 'ng-zorro-antd/drawer';
import {NzDropDownModule} from 'ng-zorro-antd/dropdown';
import {NzEmptyModule} from 'ng-zorro-antd/empty';
import {NzFormModule} from 'ng-zorro-antd/form';
import {NzGridModule} from 'ng-zorro-antd/grid';
import {NzIconModule} from 'ng-zorro-antd/icon';
import {NzInputModule} from 'ng-zorro-antd/input';
import {NzPipesModule} from 'ng-zorro-antd/pipes';
import {NzInputNumberModule} from 'ng-zorro-antd/input-number';
import {NzLayoutModule} from 'ng-zorro-antd/layout';
import {NzListModule} from 'ng-zorro-antd/list';
import {NzMenuModule} from 'ng-zorro-antd/menu';
import {NzModalModule} from 'ng-zorro-antd/modal';
import {NzNotificationServiceModule} from 'ng-zorro-antd/notification';
import {NzPageHeaderModule} from 'ng-zorro-antd/page-header';
import {NzPaginationModule} from 'ng-zorro-antd/pagination';
import {NzPopoverModule} from 'ng-zorro-antd/popover';
import {NzProgressModule} from 'ng-zorro-antd/progress';
import {NzRadioModule} from 'ng-zorro-antd/radio';
import {NzResultModule} from 'ng-zorro-antd/result';
import {NzSelectModule} from 'ng-zorro-antd/select';
import {NzSkeletonModule} from 'ng-zorro-antd/skeleton';
import {NzSliderModule} from 'ng-zorro-antd/slider';
import {NzSpaceModule} from 'ng-zorro-antd/space';
import {NzSpinModule} from 'ng-zorro-antd/spin';
import {NzStatisticModule} from 'ng-zorro-antd/statistic';
import {NzStepsModule} from 'ng-zorro-antd/steps';
import {NzSwitchModule} from 'ng-zorro-antd/switch';
import {NzTableModule} from 'ng-zorro-antd/table';
import {NzTabsModule} from 'ng-zorro-antd/tabs';
import {NzTagModule} from 'ng-zorro-antd/tag';
import {NzToolTipModule} from 'ng-zorro-antd/tooltip';
import {NzTimelineModule} from 'ng-zorro-antd/timeline';
import {NzTreeModule} from 'ng-zorro-antd/tree';
import {NzTypographyModule} from 'ng-zorro-antd/typography';
import {NzUploadModule} from 'ng-zorro-antd/upload';
import {NzCascaderModule} from "ng-zorro-antd/cascader";
import {NzTreeViewModule} from "ng-zorro-antd/tree-view";
import {NzSegmentedModule} from "ng-zorro-antd/segmented";
import {CommonModule} from "@angular/common";
import {HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {SpinnerComponent} from "./components/spinner/spinner.component";
import {ModuleWithProviders, NgModule} from "@angular/core";
import {NavMenuComponent} from "../nav-menu/nav-menu.component";

const ngZorroModule = [
  NzIconModule,
  NzButtonModule,
  NzSelectModule,
  NzLayoutModule,
  NzTableModule,
  NzCheckboxModule,
  NzGridModule,
  NzSpinModule,
  NzCardModule,
  NzTabsModule,
  NzStatisticModule,
  NzDropDownModule,
  NzMenuModule,
  NzDrawerModule,
  NzAvatarModule,
  NzNotificationServiceModule,
  NzDividerModule,
  NzToolTipModule,
  NzTypographyModule,
  NzModalModule,
  NzPageHeaderModule,
  NzBreadCrumbModule,
  NzDatePickerModule,
  NzCalendarModule,
  NzBadgeModule,
  NzTagModule,
  NzCollapseModule,
  NzSegmentedModule,
  NzInputModule,
  NzPipesModule,
  NzPaginationModule,
  NzStepsModule,
  NzAlertModule,
  NzResultModule,
  NzUploadModule,
  NzTimelineModule,
  NzDescriptionsModule,
  NzPopconfirmModule,
  NzSwitchModule,
  NzPopoverModule,
  NzTreeModule,
  NzTreeViewModule,
  NzRadioModule,
  NzCarouselModule,
  NzProgressModule,
  NzFormModule,
  NzListModule,
  NzAffixModule,
  NzSliderModule,
  NzEmptyModule,
  NzInputNumberModule,
  NzAnchorModule,
  NzSkeletonModule,
  NzSpaceModule,
  NzAutocompleteModule,
  NzPipesModule,
  NzCascaderModule
];

const modules = [
  CommonModule,
  HttpClientModule,
  FormsModule,
  ReactiveFormsModule,
  ...ngZorroModule,
];


@NgModule({
  providers: [ ],
  imports: [...modules],
  declarations: [
    SpinnerComponent,
    NavMenuComponent
  ],
  exports: [
    ...modules,
    SpinnerComponent,
  ],
})
export class SharedModule {
  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule,
      providers: [],
    };
  }
}
