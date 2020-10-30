import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SystemMenuRoutes } from './setup-menu-routes';

@NgModule({
  imports: [RouterModule.forChild(SystemMenuRoutes)],
  exports: [RouterModule]
})
export class SetupMenuRoutingModule { }
