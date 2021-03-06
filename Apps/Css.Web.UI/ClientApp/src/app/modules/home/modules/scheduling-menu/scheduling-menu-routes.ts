import { Routes, RunGuardsAndResolvers } from '@angular/router';
import { PermissionsGuard } from 'src/app/core/guards/permissions.guard';
import { AgentAdminListComponent } from './components/agent-admin/agent-admin-list/agent-admin-list.component';
import { MoveAgentsComponent } from './components/move-agents/move-agents/move-agents.component';
import { ForecastScreenListComponent } from './components/forecast-screen/forecast-screen-list/forecast-screen-list.component';
import { EntityHierarchyComponent } from './components/entity-hierarchy/entity-hierarchy.component';
import { ViewOuScreenComponent } from './components/view-ou-screen/view-ou-screen.component';
import { SchedulingGridComponent } from './components/scheduling-grid/scheduling-grid.component';
import { SchedulingManagerComponent } from './components/scheduling-manager/scheduling-manager.component';
import { AgentCategoryValueComponent } from './components/agent-category-value/agent-category-value.component';


export const SchedulingMenuRoutes: Routes = [
    {
        path: '',
        redirectTo: 'agent-admin',
        pathMatch: 'full'
    },
    {
        path: 'agent-admin',
        component: AgentAdminListComponent,
        canActivate: [PermissionsGuard],
        data: { permissions: [1, 2, 3] },
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'move-agents',
        component: MoveAgentsComponent,
        canActivate: [PermissionsGuard],
        data: { permissions: [1, 2, 3] },
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    
    {
        path: 'agent-category-value',
        component: AgentCategoryValueComponent,
        canActivate: [PermissionsGuard],
        data: { Permissions: [1, 2, 3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'scheduling-grid',
        component: SchedulingGridComponent,
        canActivate: [PermissionsGuard],
        data: { permissions: [1, 2, 3] },
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'forecast-screen',
        component: ForecastScreenListComponent,
        canActivate: [PermissionsGuard],
        data: { permissions: [1, 2, 3] },
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'view-ou-screen',
        component: ViewOuScreenComponent,
        canActivate: [PermissionsGuard],
        data: { permissions: [1, 2, 3, 4] },
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'scheduling-manager',
        component: SchedulingManagerComponent,
        canActivate: [PermissionsGuard],
        data: { permissions: [1, 2, 3, 4] },
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'entity-hierarchy',
        component: EntityHierarchyComponent,
        canActivate: [PermissionsGuard],
        data: { permissions: [1, 2, 3, 4] },
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    }


];
