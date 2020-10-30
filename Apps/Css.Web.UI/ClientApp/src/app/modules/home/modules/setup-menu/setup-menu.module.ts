import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SetupMenuRoutingModule } from './setup-menu-routing.module';

import { ClientNameListService } from './services/client-name-list.service';
import { ClientLobGroupListService } from './services/client-lob-group-list.service';
import { ClientLobGroupDropdownService } from './services/client-lob-group-dropdown.service';

import { ClientNameListComponent } from './components/client-name-list/client-name-list.component';
import { UpdateClientListComponent } from './components/update-client-list/update-client-list.component';
import { SkillTagsComponent } from './components/skill-tags/skill-tags.component';
import { UpdateSkillTagsComponent } from './components/update-skill-tags/update-skill-tags.component';
import { ClientLobGroupListComponent } from './components/client-lob-group-list/client-lob-group-list.component';
import { AddEditClientLobGroupComponent } from './components/add-edit-client-lob-group/add-edit-client-lob-group.component';
import { AddEditAgentSchedulingGroupComponent } from './components/add-edit-agent-scheduling-group/add-edit-agent-scheduling-group.component';
import { AgentSchedulingGroupListComponent } from './components/agent-scheduling-group-list/agent-scheduling-group-list.component';
import { DropdownListService } from './services/dropdown-list.service';
import { AgentSchedulingGroupService } from './services/agent-scheduling-group.service';
import { SkillGroupListComponent } from './components/skill-group-list/skill-group-list.component';
import { AddEditSkillGroupComponent } from './components/add-edit-skill-group/add-edit-skill-group.component';
import { SkillGroupsService } from './services/skill-groups.service';
import { SkillTagsService } from './services/skill-tags.service';

const modules = [SharedModule, SetupMenuRoutingModule];
const components = [
  ClientNameListComponent,
  UpdateClientListComponent,
  SkillTagsComponent,
  UpdateSkillTagsComponent,
  ClientLobGroupListComponent,
  AddEditClientLobGroupComponent,
  AgentSchedulingGroupListComponent,
  AddEditAgentSchedulingGroupComponent,
  SkillGroupListComponent,
  AddEditSkillGroupComponent
];
const providers = [
  ClientNameListService,
  ClientLobGroupListService,
  ClientLobGroupDropdownService,
  AgentSchedulingGroupService,
  DropdownListService,
  SkillGroupsService,
  SkillTagsService
];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SetupMenuModule {}
