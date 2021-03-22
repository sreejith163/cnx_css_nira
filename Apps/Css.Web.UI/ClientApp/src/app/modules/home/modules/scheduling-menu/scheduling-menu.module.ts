import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SchedulingMenuRoutingModule } from './scheduling-menu-routing.module';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { AgentAdminListComponent } from './components/agent-admin/agent-admin-list/agent-admin-list.component';
import { AddAgentProfileComponent } from './components/agent-admin/add-agent-profile/add-agent-profile.component';
import { EditAgentProfileComponent } from './components/agent-admin/edit-agent-profile/edit-agent-profile.component';
import { SchedulingGridComponent } from './components/scheduling-grid/scheduling-grid.component';
import { ImportScheduleComponent } from './components/shared/import-schedule/import-schedule.component';
import { CopyScheduleComponent } from './components/shared/copy-schedule/copy-schedule.component';
import { MoveAgentsComponent } from './components/move-agents/move-agents/move-agents.component';
import { SchedulingFilterComponent } from './components/shared/scheduling-filter/scheduling-filter.component';
import { ForecastScreenListComponent } from './components/forecast-screen/forecast-screen-list/forecast-screen-list.component';
import { ActivityLogsScheduleComponent } from './components/shared/activity-logs-schedule/activity-logs-schedule.component';
import { ViewOuScreenComponent } from './components/view-ou-screen/view-ou-screen.component';
import { SchedulingManagerComponent } from './components/scheduling-manager/scheduling-manager.component';
import { ActivityLogsComponent } from './components/agent-admin/activity-logs/activity-logs.component';
import { DateRangePopUpComponent } from './components/shared/date-range-pop-up/date-range-pop-up.component';
import { MoveAgentsListComponent } from './components/move-agents/move-agents-list/move-agents-list.component';
import { MoveAgentsSchedulingGroupTypeaheadComponent } from './components/move-agents/move-agents-scheduling-group-typeahead/move-agents-scheduling-group-typeahead.component';
import { EntityHierarchyComponent } from './components/entity-hierarchy/entity-hierarchy.component';

import { AgentAdminService } from './services/agent-admin.service';
import { AgentSchedulesService } from './services/agent-schedules.service';
import { AgentScheduleManagersService } from './services/agent-schedule-managers.service';
import { EntityHierarchyService } from './services/entity-hierarchy.service';
import { ActivityLogsService } from './services/activity-logs.service';
import { MoveAgentsService } from './services/move-agents.service';

import { NumericDirective } from 'src/app/shared/directives/numeric.directive';
import { ContenteditableValueAccessor } from 'src/app/shared/directives/contenteditable.directive';

import { HorizontalScrollPipe } from './pipes/horizontal-scroll.pipe';
import { BypassHtmlPipe } from 'src/app/shared/directives/bypassHtml.pipe';
import { TimeOffsListComponent } from './components/time-offs/time-offs-list/time-offs-list.component';
import { AddUpdateTimeOffsComponent } from './components/time-offs/add-update-time-offs/add-update-time-offs.component';
import { TimeOffsService } from './services/time-offs.service';

const modules = [DragDropModule, SharedModule, SchedulingMenuRoutingModule, BsDropdownModule.forRoot()];
const components =
  [AgentAdminListComponent,
    AddAgentProfileComponent,
    EditAgentProfileComponent,
    SchedulingGridComponent,
    ActivityLogsScheduleComponent,
    ImportScheduleComponent,
    CopyScheduleComponent,
    SchedulingManagerComponent,
    MoveAgentsComponent,
    SchedulingFilterComponent,
    EntityHierarchyComponent,
    HorizontalScrollPipe,
    ForecastScreenListComponent,
    DateRangePopUpComponent,
    ViewOuScreenComponent,
    NumericDirective,
    ActivityLogsComponent,
    MoveAgentsListComponent,
    MoveAgentsSchedulingGroupTypeaheadComponent,
    TimeOffsListComponent,
    AddUpdateTimeOffsComponent,
    ContenteditableValueAccessor,
    BypassHtmlPipe
  ];
const providers = [AgentAdminService, AgentSchedulesService, ActivityLogsService, EntityHierarchyService, AgentScheduleManagersService,
                   MoveAgentsService, TimeOffsService];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class SchedulingMenuModule { }
