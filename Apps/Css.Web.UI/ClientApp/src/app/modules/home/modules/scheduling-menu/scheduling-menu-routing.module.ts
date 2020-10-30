import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SchedulingMenuRoutes } from './scheduling-menu-routes';

@NgModule({
  imports: [RouterModule.forChild(SchedulingMenuRoutes)],
  exports: [RouterModule]
})
export class SchedulingMenuRoutingModule { }
