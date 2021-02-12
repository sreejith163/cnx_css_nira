import { RunGuardsAndResolvers } from '@angular/router';
import { PermissionsGuard } from 'src/app/core/guards/permissions.guard';
import { LanguagePreferenceResolver } from 'src/app/shared/resolvers/language-preference.resolver';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HomeComponent } from './layout/home/home.component';

export const HomeRoutes = [
    {
        path: '',
        component: HomeComponent,
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full',
            },
            {
                path: 'dashboard',
                component: DashboardComponent
            },
            {
                path: 'system-admin',
                loadChildren: () => import('./modules/system-admin/system-admin.module').then(m => m.SystemAdminModule),
                runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
                resolve: { languagePreference: LanguagePreferenceResolver }
            },
            {
                path: 'setup-menu',
                loadChildren: () => import('./modules/setup-menu/setup-menu.module').then(m => m.SetupMenuModule),
                runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
                resolve: { languagePreference: LanguagePreferenceResolver }
            },
            {
                path: 'scheduling-menu',
                loadChildren: () => import('./modules/scheduling-menu/scheduling-menu.module').then(m => m.SchedulingMenuModule),
                runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
                resolve: { languagePreference: LanguagePreferenceResolver }
            },
            {
                path: 'agent-schedule-menu',
                loadChildren: () => import('./modules/agent-schedule-menu/agent-schedule-menu.module').then(m => m.AgentScheduleMenuModule),
                runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
                resolve: { languagePreference: LanguagePreferenceResolver }
            }
        ]
    }
];
