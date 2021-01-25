import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AgentScheduleMenuRoutes } from './agent-schedule-menu-routes';

@NgModule({
  imports: [RouterModule.forChild(AgentScheduleMenuRoutes)],
  exports: [RouterModule]
})
export class AgentScheduleMenuRoutingModule { }
