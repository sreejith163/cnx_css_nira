import { Routes, RunGuardsAndResolvers } from '@angular/router';
import { PermissionsGuard } from 'src/app/core/guards/permissions.guard';
import { AgentSchedulingGroupListComponent } from './components/agent-scheduling-group/agent-scheduling-group-list/agent-scheduling-group-list.component';
import { ClientLobGroupListComponent } from './components/client-lob-group/client-lob-group-list/client-lob-group-list.component';
import { ClientNameListComponent } from './components/client-name/client-name-list/client-name-list.component';
import { SkillGroupListComponent } from './components/skill-group/skill-group-list/skill-group-list.component';
import { SkillTagsListComponent } from './components/skill-tags/skill-tags-list/skill-tags-list.component';

export const SystemMenuRoutes: Routes = [
    {
        path: '',
        redirectTo: 'client-name',
        pathMatch: 'full'
    },
    {
        path: 'client-name',
        component: ClientNameListComponent,
        canActivate: [PermissionsGuard],
        data: {permissions: [1,2,3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'skill-tags',
        component: SkillTagsListComponent,
        canActivate: [PermissionsGuard],
        data: {permissions: [1,2,3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'client-lob-group',
        component: ClientLobGroupListComponent,
        canActivate: [PermissionsGuard],
        data: {permissions: [1,2,3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'agent-scheduling-group',
        component: AgentSchedulingGroupListComponent,
        canActivate: [PermissionsGuard],
        data: {permissions: [1,2,3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    },
    {
        path: 'skill-groups',
        component: SkillGroupListComponent,
        canActivate: [PermissionsGuard],
        data: {permissions: [1,2,3]},
        runGuardsAndResolvers: 'always' as RunGuardsAndResolvers,
    }
];
