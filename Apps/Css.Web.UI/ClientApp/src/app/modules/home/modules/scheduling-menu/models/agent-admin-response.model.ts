import { AgentAdminBase } from './agent-admin-base.model';

export class AgentAdminResponse extends AgentAdminBase {
    clientId: number;
    clientName: string;
    clientLobGroupId: number;
    clientLobGroupName: string;
    skillGroupId: number;
    skillGroupName: string;
    skillTagName: string;
}