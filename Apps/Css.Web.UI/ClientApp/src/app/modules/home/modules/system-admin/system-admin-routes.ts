import { Routes, RunGuardsAndResolvers } from '@angular/router';
import { PermissionsGuard } from 'src/app/core/guards/permissions.guard';
import { AgentCategoryListComponent } from './components/agent-category/agent-category-list/agent-category-list.component';

import { PermissionsListComponent } from './components/permissions/permissions-list/permissions-list.component';
import { SchedulingCodeListComponent } from './components/scheduling-code/scheduling-code-list/scheduling-code-list.component';
import { TranslationListComponent } from './components/translation/translation-list/translation-list.component';

export const SystemAdminRoutes: Routes = [
    {
        path: '',
        redirectTo: 'transalations',
        pathMatch: 'full'
    },
    {
        path: 'transalations',
        component: TranslationListComponent
    },
    {
        path: 'agent-categories',
        canActivate: [PermissionsGuard],
        data: {permissions: [1, 2, 3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
        component: AgentCategoryListComponent
    },
    {
        path: 'scheduling-codes',
        canActivate: [PermissionsGuard],
        data: {permissions: [1, 2, 3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
        component: SchedulingCodeListComponent
    },
    {
        path: 'permissions',
        canActivate: [PermissionsGuard],
        data: {permissions: [1, 2]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
        component: PermissionsListComponent
    }
];
