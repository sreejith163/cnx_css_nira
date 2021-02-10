import { Routes, RunGuardsAndResolvers } from '@angular/router';
import { PermissionsGuard } from 'src/app/core/guards/permissions.guard';
import { AgentAdminListComponent } from './components/agent-admin/agent-admin-list/agent-admin-list.component';
import { MoveAgentsComponent } from './components/move-agents/move-agents/move-agents.component';
import { SchedulingGridComponent } from './components/scheduling-grid/scheduling-grid/scheduling-grid.component';

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
        path: 'move-agents',
        component: MoveAgentsComponent
    },
    {
        path: 'scheduling-grid',
        component: SchedulingGridComponent,
        canActivate: [PermissionsGuard],
        data: {permissions: [1]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    }
];
