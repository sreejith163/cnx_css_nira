import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class AgentShceduleMangerData {
    employeeId: string;
    agentScheduleManagerCharts: AgentScheduleManagerChart[];

    constructor() {
        this.agentScheduleManagerCharts = [];
    }
}
