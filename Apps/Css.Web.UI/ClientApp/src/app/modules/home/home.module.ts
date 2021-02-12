import { NgModule } from '@angular/core';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { DragDropModule } from '@angular/cdk/drag-drop';

import { HomeComponent } from './layout/home/home.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HeaderComponent } from './layout/home/header/header.component';
import { FooterComponent } from './layout/home/footer/footer.component';
import { SidebarMenuComponent } from './layout/home/sidebar-menu/sidebar-menu.component';
import { ControlSidebarComponent } from './layout/home/control-sidebar/control-sidebar.component';


const modules = [DragDropModule, SharedModule, HomeRoutingModule];
const components = [HomeComponent, HeaderComponent, FooterComponent, SidebarMenuComponent, DashboardComponent, ControlSidebarComponent];
const providers = [];

@NgModule({
  declarations: components,
  imports: modules,
  providers
})
export class HomeModule { }
