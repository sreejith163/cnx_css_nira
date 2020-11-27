import { Routes } from '@angular/router';
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
        component: ClientNameListComponent
    },
    {
        path: 'skill-tags',
        component: SkillTagsListComponent
    },
    {
        path: 'client-lob-group',
        component: ClientLobGroupListComponent
    },
    {
        path: 'agent-scheduling-group',
        component: AgentSchedulingGroupListComponent
    },
    {
        path: 'skill-groups',
        component: SkillGroupListComponent
    }
];
