import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class AgentShceduleMangerData {
    employeeId: number;
    agentScheduleManagerCharts: AgentScheduleManagerChart[];

    constructor() {
        this.agentScheduleManagerCharts = [];
    }
}
