import { AgentAdminAgentData } from './agent-admin-agent-data.model';

export class AgentInfo {
    scheduleType: string;
    supervisorName: string;
    supervisorId: number;
    employeeId: number;
    hireDate: string;
    productionDate: string;
    termDate: string;
    loaDate: string;
    primaryCtSkill: string;
    tourBand: string;
    rank: string;
    workMode: string;
    operationsManager: string;
    firstName: string;
    lastName: string;
    agentData: AgentAdminAgentData[];

    constructor() {
        this.agentData = [];
    }
}
