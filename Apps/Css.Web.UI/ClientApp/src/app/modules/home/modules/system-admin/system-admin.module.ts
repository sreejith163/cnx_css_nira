import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SystemAdminRoutingModule } from './system-admin-routing.module';

import { SchedulingCodeService } from './services/scheduling-code.service';
import { SchedulingCodeTypesService } from './services/scheduling-code-types.service';
import { AgentAdminDropdownsService } from '../scheduling-menu/services/agent-admin-dropdowns.service';
import { SchedulingCodeIconsService } from './services/scheduling-code-icons.service';
import { PermissionsService } from './services/permissions.service';
import { AgentCategoryService } from './services/agent-category.service';
import { CssMenuService } from './services/css-menu.service';

import { SchedulingCodeListComponent } from './components/scheduling-code/scheduling-code-list/scheduling-code-list.component';
import { AddUpdateSchedulingCodeComponent } from './components/scheduling-code/add-update-scheduling-code/add-update-scheduling-code.component';
import { PermissionsListComponent } from './components/permissions/permissions-list/permissions-list.component';
import { EmployeeTypeAheadComponent } from './components/permissions/employee-typeahead/employee-typeahead.component';
import { AddUpdatePermissionComponent } from './components/permissions/add-update-permission/add-update-permission.component';
import { AgentCategoryListComponent } from './components/agent-category/agent-category-list/agent-category-list.component';
import { AddAgentCategoryComponent } from './components/agent-category/add-agent-category/add-agent-category.component';
import { TranslationListComponent } from './components/translation/translation-list/translation-list.component';
import { AddUpdateTranslationsComponent } from './components/translation/add-update-translations/add-update-translations.component';

const modules = [
  SharedModule,
  SystemAdminRoutingModule];

const components = [
  TranslationListComponent,
  AddUpdateTranslationsComponent,
  AgentCategoryListComponent,
  SchedulingCodeListComponent,
  AddUpdateSchedulingCodeComponent,
  PermissionsListComponent,
  AddUpdatePermissionComponent,
  EmployeeTypeAheadComponent,
  AddAgentCategoryComponent];

const providers = [
  SchedulingCodeService,
  SchedulingCodeTypesService,
  AgentAdminDropdownsService,
  SchedulingCodeIconsService,
  PermissionsService,
  AgentCategoryService,
  CssMenuService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SystemAdminModule { }
