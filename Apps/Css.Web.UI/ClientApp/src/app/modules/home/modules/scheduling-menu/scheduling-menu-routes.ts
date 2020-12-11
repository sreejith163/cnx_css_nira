import { Routes } from '@angular/router';
import { AgentAdminListComponent } from './components/agent-admin/agent-admin-list/agent-admin-list.component';
import { SchedulingGridComponent } from './components/scheduling-grid/scheduling-grid.component';

export const SchedulingMenuRoutes: Routes = [
    {
        path: '',
        redirectTo: 'agent-admin',
        pathMatch: 'full'
    },
    {
        path: 'agent-admin',
        component: AgentAdminListComponent
    },
    {
        path: 'scheduling-grid',
        component: SchedulingGridComponent
    }
];
