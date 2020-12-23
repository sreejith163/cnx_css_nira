import { AgentAdminAgentData } from './agent-admin-agent-data.model';

export class AgentAdminBase {
    agentAdminId: number;
    employeeId: number;
    sso: string;
    skillTagId: number;
    firstName: string;
    lastName: string;
    agentData: AgentAdminAgentData[];
}
