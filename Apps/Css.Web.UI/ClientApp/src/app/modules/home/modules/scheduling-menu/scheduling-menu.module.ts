import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SchedulingMenuRoutingModule } from './scheduling-menu-routing.module';

import { HorizontalScrollPipe } from './pipes/horizontal-scroll.pipe';

import { AgentAdminService } from './services/agent-admin.service';
import { AgentSchedulesService } from './services/agent-schedules.service';
import { ActivityLogsService } from './services/activity-logs.service';

import { SchedulingGridComponent } from './components/scheduling-grid/scheduling-grid/scheduling-grid.component';
import { ImportScheduleComponent } from './components/scheduling-grid/import-schedule/import-schedule.component';
import { CopyScheduleComponent } from './components/scheduling-grid/copy-schedule/copy-schedule.component';
import { SchedulingManagerComponent } from './components/scheduling-grid/scheduling-manager/scheduling-manager.component';
import { MoveAgentsComponent } from './components/move-agents/move-agents/move-agents.component';
import { SchedulingComponent } from './components/scheduling-grid/scheduling/scheduling.component';
import { SchedulingFilterComponent } from './components/scheduling-grid/scheduling-filter/scheduling-filter.component';
import { AgentAdminListComponent } from './components/agent-admin/agent-admin-list/agent-admin-list.component';
import { AddAgentProfileComponent } from './components/agent-admin/add-agent-profile/add-agent-profile.component';
import { ActivityLogsComponent } from './components/scheduling-grid/activity-logs/activity-logs.component';


const modules = [DragDropModule, SharedModule, SchedulingMenuRoutingModule];
const components =
  [AgentAdminListComponent,
    AddAgentProfileComponent,
    SchedulingGridComponent,
    ActivityLogsComponent,
    ImportScheduleComponent,
    CopyScheduleComponent,
    SchedulingManagerComponent,
    MoveAgentsComponent,
    SchedulingComponent,
    SchedulingFilterComponent,
    HorizontalScrollPipe];
const providers = [AgentAdminService, AgentSchedulesService, ActivityLogsService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SchedulingMenuModule { }
