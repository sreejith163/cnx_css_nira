import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SchedulingMenuRoutingModule } from './scheduling-menu-routing.module';

import { AgentAdminListComponent } from './components/agent-admin/agent-admin-list/agent-admin-list.component';
import { AddAgentProfileComponent } from './components/agent-admin/add-agent-profile/add-agent-profile.component';
import { EditAgentProfileComponent } from './components/agent-admin/edit-agent-profile/edit-agent-profile.component';

import { HorizontalScrollPipe } from './pipes/horizontal-scroll.pipe';

import { AgentAdminService } from './services/agent-admin.service';
import { AgentSchedulesService } from './services/agent-schedules.service';
import { SchedulingGridComponent } from './components/scheduling-grid/scheduling-grid/scheduling-grid.component';
import { ImportScheduleComponent } from './components/shared/import-schedule/import-schedule.component';
import { CopyScheduleComponent } from './components/shared/copy-schedule/copy-schedule.component';
import { MoveAgentsComponent } from './components/move-agents/move-agents/move-agents.component';

import { SchedulingFilterComponent } from './components/shared/scheduling-filter/scheduling-filter.component';

// for optimization and code cleaning
import { ForecastScreenListComponent } from './components/forecast-screen/forecast-screen-list/forecast-screen-list.component';
import { FilterComponent } from './components/forecast-screen/filter/filter.component';
import { ActivityLogsScheuldeComponent } from './components/shared/activity-logs-schedule/activity-logs-schedule.component';
import { ViewOuScreenComponent } from './components/view-ou-screen/view-ou-screen.component';


import { EntityHierarchyService } from './services/entity-hierarchy.service';
import { EntityHierarchyComponent } from './components/entity-hierarchy/entity-hierarchy.component';
import { NumericDirective } from 'src/app/shared/directives/numeric.directive';
import { ActivityLogsService } from './services/activity-logs.service';
import { SchedulingManagerComponent } from './components/scheduling-manager/scheduling-manager/scheduling-manager.component';
const modules = [DragDropModule, SharedModule, SchedulingMenuRoutingModule];
const components =
  [AgentAdminListComponent,
    AddAgentProfileComponent,
    EditAgentProfileComponent,
    SchedulingGridComponent,
    ActivityLogsScheuldeComponent,
    ImportScheduleComponent,
    CopyScheduleComponent,
    SchedulingManagerComponent,
    MoveAgentsComponent,
    SchedulingFilterComponent,
    EntityHierarchyComponent,
    HorizontalScrollPipe,
    ForecastScreenListComponent,
    FilterComponent,
    ViewOuScreenComponent,
    NumericDirective
  ];
const providers = [AgentAdminService, AgentSchedulesService, ActivityLogsService, EntityHierarchyService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SchedulingMenuModule { }
