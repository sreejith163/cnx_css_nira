import { AgentShceduleMangerData } from './agent-schedule-manager-data.model';

export class UpdateAgentScheduleMangersChart {
    agentScheduleManagers: AgentShceduleMangerData[];
    modifiedBy: string;

    constructor() {
        this.agentScheduleManagers = [];
    }
}
