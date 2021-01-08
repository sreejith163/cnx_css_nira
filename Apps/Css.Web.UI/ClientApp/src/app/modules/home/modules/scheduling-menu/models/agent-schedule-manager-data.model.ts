import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class AgentShceduleMangerData {
    employeeId: number;
    agentScheduleMangerChart: AgentScheduleManagerChart[];

    constructor() {
        this.agentScheduleMangerChart = [];
    }
}
