import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SystemAdminRoutes } from './system-admin-routes';

@NgModule({
  imports: [RouterModule.forChild(SystemAdminRoutes)],
  exports: [RouterModule]
})
export class SystemAdminRoutingModule { }
