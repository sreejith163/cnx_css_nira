import { Routes } from '@angular/router';
import { AgentCategoryListComponent } from './components/agent-category/agent-category-list/agent-category-list.component';

import { PermissionsListComponent } from './components/permissions/permissions-list/permissions-list.component';
import { SchedulingCodeListComponent } from './components/scheduling-code/scheduling-code-list/scheduling-code-list.component';
import { TranslationListComponent } from './components/translation-list/translation-list.component';

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
        component: AgentCategoryListComponent
    },
    {
        path: 'scheduling-codes',
        component: SchedulingCodeListComponent
    },
    {
        path: 'permissions',
        component: PermissionsListComponent
    }
];
