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
                pathMatch: 'full'
            },
            {
                path: 'dashboard',
                component: DashboardComponent,
            },
            {
                path: 'system-admin',
                loadChildren: () => import('./modules/system-admin/system-admin.module').then(m => m.SystemAdminModule)
            },
            {
                path: 'setup-menu',
                loadChildren: () => import('./modules/setup-menu/setup-menu.module').then(m => m.SetupMenuModule)
            },
            {
                path: 'scheduling-menu',
                loadChildren: () => import('./modules/scheduling-menu/scheduling-menu.module').then(m => m.SchedulingMenuModule)
            }
        ]
    }
];
