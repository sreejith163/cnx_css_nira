import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SortDirective } from './directives/sort.directive';
import { TranslationPipe } from './pipes/translation.pipe';

import { GenericStateManagerService } from './services/generic-state-manager.service';

import { MessagePopUpComponent } from './popups/message-pop-up/message-pop-up.component';
import { ConfirmationPopUpComponent } from './popups/confirmation-pop-up/confirmation-pop-up.component';

const components = [ConfirmationPopUpComponent, ConfirmationPopUpComponent, SortDirective, TranslationPipe];

const modalComponents = [ConfirmationPopUpComponent, MessagePopUpComponent];

const modules = [CommonModule, FormsModule, ReactiveFormsModule, NgbModule];

const providers = [GenericStateManagerService];

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
