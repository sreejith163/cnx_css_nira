import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SystemAdminRoutingModule } from './system-admin-routing.module';

import { TranslationService } from './services/translation.service';
import { SchedulingCodeService } from './services/scheduling-code.service';

import { TranslationListComponent } from './components/translation-list/translation-list.component';
import { AgentCategoryListComponent } from './components/agent-category-list/agent-category-list.component';
import { AgentCategoryListService } from './services/agent-category-list.service';
import { AgentAdminDropdownsService } from '../scheduling-menu/services/agent-admin-dropdowns.service';
import { AgentCategoryDropdownService } from './services/agent-category-dropdown.service';
import { AddAgentCategoryComponent } from './components/add-agent-category/add-agent-category.component';
import { AddEditTranslationComponent } from './components/add-edit-translation/add-edit-translation.component';
import { SchedulingCodeListComponent } from './components/scheduling-code/scheduling-code-list/scheduling-code-list.component';
import { AddUpdateSchedulingCodeComponent } from './components/scheduling-code/add-update-scheduling-code/add-update-scheduling-code.component';

const modules = [
  SharedModule,
  SystemAdminRoutingModule];

const components = [
  TranslationListComponent,
  AddEditTranslationComponent,
  AgentCategoryListComponent,
  SchedulingCodeListComponent,
  AddUpdateSchedulingCodeComponent,
  AddAgentCategoryComponent];

const providers = [
  TranslationService,
  AgentCategoryListService,
  AgentAdminDropdownsService,
  SchedulingCodeService,
  AgentCategoryDropdownService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SystemAdminModule { }
