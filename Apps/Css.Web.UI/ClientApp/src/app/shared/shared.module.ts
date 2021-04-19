import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { PapaParseModule } from 'ngx-papaparse';

import { ClientLobGroupService } from '../modules/home/modules/setup-menu/services/client-lob-group.service';
import { ClientService } from '../modules/home/modules/setup-menu/services/client.service';
import { GenericDataService } from '../modules/home/modules/setup-menu/services/generic-data.service';
import { SkillGroupService } from '../modules/home/modules/setup-menu/services/skill-group.service';
import { SkillTagService } from '../modules/home/modules/setup-menu/services/skill-tag.service';
import { AgentSchedulingGroupService } from './services/agent-scheduling-group.service';
import { CssLanguageService } from './services/css-language.service';
import { GenericStateManagerService } from './services/generic-state-manager.service';
import { LanguageTranslationService } from './services/language-translation.service';
import { SchedulingCodeService } from './services/scheduling-code.service';
import { TimezoneService } from './services/timezone.service';
import { TranslationService } from './services/translation.service';
import { ExcelService } from './services/excel.service';

import { AgentSchedulingGroupTypeaheadComponent } from './components/agent-scheduling-group-typeahead/agent-scheduling-group-typeahead.component';
import { AgentCategoryTypeaheadComponent } from './components/agent-category-typeahead/agent-category-typeahead.component';
import { ClientLobGroupTypeaheadComponent } from './components/client-lob-group-typeahead/client-lob-group-typeahead.component';
import { ClientNameTypeAheadComponent } from './components/client-name-typeahead/client-name-typeahead.component';
import { SkillGroupTypeaheadComponent } from './components/skill-group-typeahead/skill-group-typeahead.component';
import { SkillTagTypeaheadComponent } from './components/skill-tag-typeahead/skill-tag-typeahead.component';
import { ConfirmationPopUpComponent } from './popups/confirmation-pop-up/confirmation-pop-up.component';
import { ErrorWarningPopUpComponent } from './popups/error-warning-pop-up/error-warning-pop-up.component';
import { MessagePopUpComponent } from './popups/message-pop-up/message-pop-up.component';
import { GenericPopUpComponent } from './popups/generic-pop-up/generic-pop-up.component';

import { TranslationPipe } from './pipes/translation.pipe';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';
import { LanguagePreferenceResolver } from './resolvers/language-preference.resolver';
import { PermissionDirective } from './directives/permission.directive';
import { SortDirective } from './directives/sort.directive';
import { DradAndDropFileDirective } from './directives/drad-and-drop-file.directive';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

const components =
  [
    ConfirmationPopUpComponent,
    ConfirmationPopUpComponent,
    ErrorWarningPopUpComponent,
    GenericPopUpComponent,
    MessagePopUpComponent,
    ClientNameTypeAheadComponent,
    ClientLobGroupTypeaheadComponent,
    SkillTagTypeaheadComponent,
    SkillGroupTypeaheadComponent,
    AgentSchedulingGroupTypeaheadComponent,
    AgentCategoryTypeaheadComponent,
    SortDirective,
    TranslationPipe,
    SafeHtmlPipe,
    TruncatePipe,
    PermissionDirective
  ];

const modalComponents = [ConfirmationPopUpComponent, MessagePopUpComponent, DradAndDropFileDirective];

const modules = [
  CommonModule,
  FormsModule,
  ReactiveFormsModule,
  NgbModule,
  NgSelectModule,
  NgxSpinnerModule,
  PapaParseModule,
  TabsModule,
  ScrollingModule,
  TranslateModule.forRoot({
    loader: {
      provide: TranslateLoader,
      useFactory: (HttpLoaderFactory),
      deps: [HttpClient]
    }
  })];

const providers = [GenericStateManagerService, TranslationService, TimezoneService, CssLanguageService, LanguageTranslationService,
  GenericDataService, ClientService, SkillGroupService, ClientLobGroupService, SkillTagService, AgentSchedulingGroupService,
  SchedulingCodeService, ExcelService, LanguagePreferenceResolver ];

@NgModule({
  imports: modules,
  declarations: components,
  entryComponents: modalComponents,
  exports: [components, modules]
})

export class SharedModule {
  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule,
      providers
    };
  }
}
