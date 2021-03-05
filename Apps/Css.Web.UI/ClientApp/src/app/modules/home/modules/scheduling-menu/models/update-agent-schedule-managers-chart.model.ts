import { AgentShceduleMangerData } from './agent-schedule-manager-data.model';

export class UpdateAgentScheduleMangersChart {
    agentScheduleManagers: AgentShceduleMangerData[];
    date: Date;
    activityOrigin: number;
    modifiedUser: number;
    modifiedBy: string;
    isImport: boolean;

    constructor() {
        this.agentScheduleManagers = [];
    }
}
