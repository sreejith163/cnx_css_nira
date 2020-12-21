import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SchedulingMenuRoutingModule } from './scheduling-menu-routing.module';

import { AgentAdminListComponent } from './components/agent-admin/agent-admin-list/agent-admin-list.component';
import { AddAgentProfileComponent } from './components/agent-admin/add-agent-profile/add-agent-profile.component';
import { SchedulingGridComponent } from './components/scheduling-grid/scheduling-grid.component';
import { HorizontalScrollPipe } from './pipes/horizontal-scroll.pipe';

import { AgentAdminService } from './services/agent-admin.service';
import { AgentSchedulesService } from './services/agent-schedules.service';


const modules = [DragDropModule, SharedModule, SchedulingMenuRoutingModule];
const components = [AgentAdminListComponent, AddAgentProfileComponent, SchedulingGridComponent, HorizontalScrollPipe];
const providers = [AgentAdminService, AgentSchedulesService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SchedulingMenuModule { }
