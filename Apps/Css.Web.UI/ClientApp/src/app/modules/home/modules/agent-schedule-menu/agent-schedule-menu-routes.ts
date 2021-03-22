import { Routes } from '@angular/router';
import { PermissionsGuard } from 'src/app/core/guards/permissions.guard';
import { AgentScheduleComponent } from './components/agent-schedule/agent-schedule.component';


export const AgentScheduleMenuRoutes: Routes = [
    {
        path: '',
        redirectTo: 'my-schedule',
        pathMatch: 'full'
    },
    {
        path: 'my-schedule',
        canActivate: [PermissionsGuard],
        data: {permissions: [0]},
        component: AgentScheduleComponent
    },
];
