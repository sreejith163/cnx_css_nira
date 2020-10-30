import { Routes } from '@angular/router';

import { SchedulingCodeListComponent } from './components/scheduling-code-list/scheduling-code-list.component';
import { AgentCategoryListComponent } from './components/agent-category-list/agent-category-list.component';
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
    }
];
