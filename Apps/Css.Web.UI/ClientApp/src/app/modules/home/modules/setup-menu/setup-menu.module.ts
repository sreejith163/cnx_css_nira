import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SetupMenuRoutingModule } from './setup-menu-routing.module';

import { DropdownListService } from './services/dropdown-list.service';
import { AgentSchedulingGroupService } from './services/agent-scheduling-group.service';
import { ClientService } from './services/client.service';
import { ClientLobGroupService } from './services/client-lob-group.service';
import { SkillGroupService } from './services/skill-group.service';
import { GenericDataService } from './services/generic-data.service';
import { SkillTagService } from './services/skill-tag.service';

import { AddUpdateClientNameComponent } from './components/client-name/add-update-client-name/add-update-client-name.component';
import { ClientNameListComponent } from './components/client-name/client-name-list/client-name-list.component';
import { AddEditAgentSchedulingGroupComponent } from './components/agent-scheduling-group/add-edit-agent-scheduling-group/add-edit-agent-scheduling-group.component';
import { AgentSchedulingGroupListComponent } from './components/agent-scheduling-group/agent-scheduling-group-list/agent-scheduling-group-list.component';
import { AddEditSkillGroupComponent } from './components/skill-group/add-edit-skill-group/add-edit-skill-group.component';
import { ClientLobGroupListComponent } from './components/client-lob-group/client-lob-group-list/client-lob-group-list.component';
import { AddUpdateClientLobGroupComponent } from './components/client-lob-group/add-update-client-lob-group/add-update-client-lob-group.component';
import { SkillGroupListComponent } from './components/skill-group/skill-group-list/skill-group-list.component';
import { SkillTagsListComponent } from './components/skill-tags/skill-tags-list/skill-tags-list.component';
import { AddUpdateSkillTagComponent } from './components/skill-tags/add-update-skill-tag/add-update-skill-tag.component';

const modules = [SharedModule, SetupMenuRoutingModule];
const components = [
  ClientNameListComponent,
  AddUpdateClientNameComponent,
  ClientLobGroupListComponent,
  AddUpdateClientLobGroupComponent,
  AgentSchedulingGroupListComponent,
  AddEditAgentSchedulingGroupComponent,
  SkillGroupListComponent,
  AddEditSkillGroupComponent,
  SkillTagsListComponent,
  AddUpdateSkillTagComponent
];

const providers = [
  ClientService,
  AgentSchedulingGroupService,
  DropdownListService,
  SkillGroupService,
  ClientLobGroupService,
  SkillTagService,
  GenericDataService
];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SetupMenuModule {}
