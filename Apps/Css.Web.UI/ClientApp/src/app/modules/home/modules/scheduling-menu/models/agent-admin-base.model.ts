import { AgentAdminAgentData } from './agent-admin-agent-data.model';

export class AgentAdminBase {
    id: string;
    employeeId: number;
    sso: string;
    skillTagId: number;
    agentSchedulingGroupId: number;
    clientId: number;
    clientLobGroupId: number;
    skillGroupId: number;
    firstName: string;
    lastName: string;
    agentData: AgentAdminAgentData[];
    supervisorId: string;
    supervisorName: string;
    supervisorSso: string;
    pto: {
        earned: string;
        credited: string;
        cofromlastyear: string;
        cofornextyear: string;
        planned: string;
        taken: string;
        debited: string;
        remaining: string;
    };
}
