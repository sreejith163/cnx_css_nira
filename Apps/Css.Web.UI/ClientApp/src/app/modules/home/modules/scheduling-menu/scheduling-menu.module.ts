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
import { ImportScheduleComponent } from './components/scheduling-grid/import-schedule/import-schedule.component';
import { CopyScheduleComponent } from './components/scheduling-grid/copy-schedule/copy-schedule.component';
import { SchedulingManagerComponent } from './components/scheduling-grid/scheduling-manager/scheduling-manager.component';
import { MoveAgentsComponent } from './components/move-agents/move-agents/move-agents.component';

import { SchedulingComponent } from './components/scheduling-grid/scheduling/scheduling.component';
import { SchedulingFilterComponent } from './components/scheduling-grid/scheduling-filter/scheduling-filter.component';

// for optimization and code cleaning
import { SchedulingGridMComponent } from './components/scheduling-manager/scheduling-grid/scheduling-grid.component';
import { ImportScheduleMComponent } from './components/scheduling-manager/import-schedule/import-schedule.component';
import { CopyScheduleMComponent } from './components/scheduling-manager/copy-schedule/copy-schedule.component';
import { SchedulingMComponent } from './components/scheduling-manager/scheduling/scheduling.component';
import { SchedulingFilterMComponent } from './components/scheduling-manager/scheduling-filter/scheduling-filter.component';
import { SchedulingManagerMComponent } from './components/scheduling-manager/scheduling-manager/scheduling-manager.component';
import { ActivityLogsComponent } from './components/scheduling-grid/activity-logs/activity-logs.component';

import { EntityHierarchyService } from './services/entity-hierarchy.service';
import { EntityHierarchyComponent } from './components/entity-hierarchy/entity-hierarchy.component';
import { ForecastScreenListComponent } from './components/forecast-screen/forecast-screen-list/forecast-screen-list.component';
import { FilterComponent } from './components/forecast-screen/filter/filter.component';
import { ViewOuScreenListComponent } from './components/view-ou-screen/view-ou-screen-list/view-ou-screen-list.component';
import { ViewOuScreenFilterComponent } from './components/view-ou-screen/view-ou-screen-filter/view-ou-screen-filter.component';
import { NumericDirective } from 'src/app/shared/directives/numeric.directive';
import { ActivityLogsService } from './services/activity-logs.service';
const modules = [DragDropModule, SharedModule, SchedulingMenuRoutingModule];
const components =
  [AgentAdminListComponent,
    AddAgentProfileComponent,
    EditAgentProfileComponent,
    SchedulingGridComponent,
    ActivityLogsComponent,
    ImportScheduleComponent,
    CopyScheduleComponent,
    SchedulingManagerComponent,
    MoveAgentsComponent,
    SchedulingComponent,
    SchedulingFilterComponent,
    SchedulingGridMComponent,
    ImportScheduleMComponent,
    CopyScheduleMComponent,
    SchedulingMComponent,
    SchedulingFilterMComponent,
    SchedulingManagerMComponent,
    EntityHierarchyComponent,
    HorizontalScrollPipe,
    ForecastScreenListComponent,
    FilterComponent,
    ViewOuScreenListComponent,
    ViewOuScreenFilterComponent,
    NumericDirective
  ];
const providers = [AgentAdminService, AgentSchedulesService, ActivityLogsService, EntityHierarchyService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SchedulingMenuModule { }
