import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxSpinnerModule } from 'ngx-spinner';

import { SortDirective } from './directives/sort.directive';
import { TranslationPipe } from './pipes/translation.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';

import { GenericStateManagerService } from './services/generic-state-manager.service';
import { TranslationService } from './services/translation.service';
import { TimezoneService } from './services/timezone.service';
import { LanguageTranslationService } from './services/language-translation.service';
import { CssLanguageService } from './services/css-language.service';

import { MessagePopUpComponent } from './popups/message-pop-up/message-pop-up.component';
import { ConfirmationPopUpComponent } from './popups/confirmation-pop-up/confirmation-pop-up.component';
import { ErrorWarningPopUpComponent } from './popups/error-warning-pop-up/error-warning-pop-up.component';

const components =
  [
    ConfirmationPopUpComponent,
    ConfirmationPopUpComponent,
    ErrorWarningPopUpComponent,
    SortDirective,
    TranslationPipe,
    TruncatePipe
  ];

const modalComponents = [ConfirmationPopUpComponent, MessagePopUpComponent];

const modules = [CommonModule, FormsModule, ReactiveFormsModule, NgbModule, NgSelectModule, NgxSpinnerModule];

const providers = [GenericStateManagerService, TranslationService, TimezoneService, CssLanguageService, LanguageTranslationService];

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
