import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ClientLobGroupService } from '../modules/home/modules/setup-menu/services/client-lob-group.service';
import { ClientService } from '../modules/home/modules/setup-menu/services/client.service';
import { GenericDataService } from '../modules/home/modules/setup-menu/services/generic-data.service';
import { SkillGroupService } from '../modules/home/modules/setup-menu/services/skill-group.service';
import { SkillTagService } from '../modules/home/modules/setup-menu/services/skill-tag.service';
import { AgentSchedulingGroupTypeaheadComponent } from './components/agent-scheduling-group-typeahead/agent-scheduling-group-typeahead.component';
import { ClientLobGroupTypeaheadComponent } from './components/client-lob-group-typeahead/client-lob-group-typeahead.component';
import { ClientNameTypeAheadComponent } from './components/client-name-typeahead/client-name-typeahead.component';
import { SkillGroupTypeaheadComponent } from './components/skill-group-typeahead/skill-group-typeahead.component';
import { SkillTagTypeaheadComponent } from './components/skill-tag-typeahead/skill-tag-typeahead.component';
import { SortDirective } from './directives/sort.directive';

import { TranslationPipe } from './pipes/translation.pipe';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';

import { ConfirmationPopUpComponent } from './popups/confirmation-pop-up/confirmation-pop-up.component';
import { ErrorWarningPopUpComponent } from './popups/error-warning-pop-up/error-warning-pop-up.component';
import { MessagePopUpComponent } from './popups/message-pop-up/message-pop-up.component';
import { AgentSchedulingGroupService } from './services/agent-scheduling-group.service';
import { CssLanguageService } from './services/css-language.service';
import { GenericStateManagerService } from './services/generic-state-manager.service';
import { LanguageTranslationService } from './services/language-translation.service';
import { SchedulingCodeService } from './services/scheduling-code.service';
import { TimezoneService } from './services/timezone.service';
import { TranslationService } from './services/translation.service';
import { ExcelService } from './services/excel.service';
import { DradAndDropFileDirective } from './directives/drad-and-drop-file.directive';

const components =
  [
    ConfirmationPopUpComponent,
    ConfirmationPopUpComponent,
    ErrorWarningPopUpComponent,
    ClientNameTypeAheadComponent,
    ClientLobGroupTypeaheadComponent,
    SkillTagTypeaheadComponent,
    SkillGroupTypeaheadComponent,
    AgentSchedulingGroupTypeaheadComponent,
    SortDirective,
    TranslationPipe,
    SafeHtmlPipe,
    TruncatePipe
  ];

const modalComponents = [ConfirmationPopUpComponent, MessagePopUpComponent, DradAndDropFileDirective];

const modules = [CommonModule, FormsModule, ReactiveFormsModule, NgbModule, NgSelectModule, NgxSpinnerModule];

const providers = [GenericStateManagerService, TranslationService, TimezoneService, CssLanguageService, LanguageTranslationService,
  GenericDataService, ClientService, SkillGroupService, ClientLobGroupService, SkillTagService, AgentSchedulingGroupService,
  SchedulingCodeService, ExcelService ];

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
