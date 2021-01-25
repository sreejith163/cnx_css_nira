import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgentScheduleComponent } from './components/agent-schedule/agent-schedule.component';
import { AgentScheduleMenuRoutingModule } from './agent-schedule-menu-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

const modules = [CommonModule, AgentScheduleMenuRoutingModule, SharedModule];

const components = [AgentScheduleComponent];

const providers = [];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class AgentScheduleMenuModule { }
