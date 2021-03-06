import { AgentShceduleMangerData } from './agent-schedule-manager-data.model';

export class UpdateAgentScheduleMangersChart {
    scheduleManagers: AgentShceduleMangerData[];
    activityOrigin: number;
    modifiedUser: number;
    modifiedBy: string;
    isImport: boolean;

    constructor() {
        this.scheduleManagers = [];
    }
}
