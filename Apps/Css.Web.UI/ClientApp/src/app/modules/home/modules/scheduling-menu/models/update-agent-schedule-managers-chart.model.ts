import { AgentShceduleMangerData } from './agent-schedule-manager-data.model';

export class UpdateAgentScheduleMangersChart {
    agentScheduleManagers: AgentShceduleMangerData[];
    activityOrigin: number;
    modifiedBy: string;

    constructor() {
        this.agentScheduleManagers = [];
    }
}
