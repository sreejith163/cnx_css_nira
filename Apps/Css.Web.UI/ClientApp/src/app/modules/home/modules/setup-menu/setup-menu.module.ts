import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SetupMenuRoutingModule } from './setup-menu-routing.module';

import { ClientLobGroupListService } from './services/client-lob-group-list.service';
import { ClientLobGroupDropdownService } from './services/client-lob-group-dropdown.service';
import { DropdownListService } from './services/dropdown-list.service';
import { AgentSchedulingGroupService } from './services/agent-scheduling-group.service';
import { SkillGroupsService } from './services/skill-groups.service';
import { SkillTagsService } from './services/skill-tags.service';
import { ClientService } from './services/client.service';

import { AddUpdateClientNameComponent } from './components/client-name/add-update-client-name/add-update-client-name.component';
import { ClientNameListComponent } from './components/client-name/client-name-list/client-name-list.component';
import { SkillTagsComponent } from './components/skill-tags/skill-tags.component';
import { UpdateSkillTagsComponent } from './components/update-skill-tags/update-skill-tags.component';
import { AddEditAgentSchedulingGroupComponent } from './components/add-edit-agent-scheduling-group/add-edit-agent-scheduling-group.component';
import { AgentSchedulingGroupListComponent } from './components/agent-scheduling-group-list/agent-scheduling-group-list.component';
import { SkillGroupListComponent } from './components/skill-group-list/skill-group-list.component';
import { AddEditSkillGroupComponent } from './components/add-edit-skill-group/add-edit-skill-group.component';
import { ClientNameTypeAheadComponent } from './components/client-name/client-name-typeahead/client-name-typeahead.component';
import { ClientLobGroupService } from './services/client-lob-group.service';
import { ClientLobGroupListComponent } from './components/client-lob-group/client-lob-group-list/client-lob-group-list.component';
import { AddUpdateClientLobGroupComponent } from './components/client-lob-group/add-update-client-lob-group/add-update-client-lob-group.component';

const modules = [SharedModule, SetupMenuRoutingModule];
const components = [
  ClientNameListComponent,
  AddUpdateClientNameComponent,
  SkillTagsComponent,
  UpdateSkillTagsComponent,
  ClientLobGroupListComponent,
  AddUpdateClientLobGroupComponent,
  AgentSchedulingGroupListComponent,
  AddEditAgentSchedulingGroupComponent,
  SkillGroupListComponent,
  AddEditSkillGroupComponent,
  ClientNameTypeAheadComponent
];
const providers = [
  ClientService,
  ClientLobGroupListService,
  ClientLobGroupDropdownService,
  AgentSchedulingGroupService,
  DropdownListService,
  SkillGroupsService,
  SkillTagsService,
  ClientLobGroupService
];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SetupMenuModule {}
