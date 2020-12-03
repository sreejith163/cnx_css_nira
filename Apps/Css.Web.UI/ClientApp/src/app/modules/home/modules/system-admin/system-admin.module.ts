import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SystemAdminRoutingModule } from './system-admin-routing.module';

import { TranslationService } from './services/translation.service';
import { SchedulingCodeService } from './services/scheduling-code.service';
import { SchedulingCodeTypesService } from './services/scheduling-code-types.service';
import { AgentAdminDropdownsService } from '../scheduling-menu/services/agent-admin-dropdowns.service';
import { SchedulingCodeIconsService } from './services/scheduling-code-icons.service';
import { PermissionsService } from './services/permissions.service';

import { TranslationListComponent } from './components/translation-list/translation-list.component';
import { AddEditTranslationComponent } from './components/add-edit-translation/add-edit-translation.component';
import { SchedulingCodeListComponent } from './components/scheduling-code/scheduling-code-list/scheduling-code-list.component';
import { AddUpdateSchedulingCodeComponent } from './components/scheduling-code/add-update-scheduling-code/add-update-scheduling-code.component';
import { PermissionsListComponent } from './components/permissions/permissions-list/permissions-list.component';
import { EmployeeTypeAheadComponent } from './components/permissions/employee-typeahead/employee-typeahead.component';
import { AddUpdatePermissionComponent } from './components/permissions/add-update-permission/add-update-permission.component';
import { AgentCategoryListComponent } from './components/agent-category/agent-category-list/agent-category-list.component';
import { AddAgentCategoryComponent } from './components/agent-category/add-agent-category/add-agent-category.component';
import { AgentCategoryService } from './services/agent-category.service';

const modules = [
  SharedModule,
  SystemAdminRoutingModule];

const components = [
  TranslationListComponent,
  AddEditTranslationComponent,
  AgentCategoryListComponent,
  SchedulingCodeListComponent,
  AddUpdateSchedulingCodeComponent,
  PermissionsListComponent,
  AddUpdatePermissionComponent,
  EmployeeTypeAheadComponent,
  AddAgentCategoryComponent];

const providers = [
  TranslationService,
  SchedulingCodeService,
  SchedulingCodeTypesService,
  AgentAdminDropdownsService,
  SchedulingCodeIconsService,
  PermissionsService,
  AgentCategoryService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SystemAdminModule { }
