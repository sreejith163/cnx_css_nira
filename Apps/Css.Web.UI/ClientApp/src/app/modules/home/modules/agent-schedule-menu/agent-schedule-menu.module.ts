import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AgentScheduleComponent } from './components/agent-schedule/agent-schedule.component';
import { AgentScheduleMenuRoutingModule } from './agent-schedule-menu-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { AgentMyScheduleService } from './services/agent-myschedule.service';
import { AgentScheduleMonthlyViewComponent } from './components/agent-schedule-monthly-view/agent-schedule-monthly-view.component';

const modules = [CommonModule, AgentScheduleMenuRoutingModule, SharedModule];

const components = [AgentScheduleComponent, AgentScheduleMonthlyViewComponent];

const providers = [AgentMyScheduleService, DatePipe];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class AgentScheduleMenuModule { }
