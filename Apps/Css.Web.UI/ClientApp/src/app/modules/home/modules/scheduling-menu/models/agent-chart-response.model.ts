import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class AgentChartResponse {
    id: string;
    agentScheduleCharts: AgentScheduleChart[];
    agentScheduleManagerCharts: AgentScheduleManagerChart[];

    constructor() {
        this.agentScheduleManagerCharts = [];
        this.agentScheduleCharts = [];
    }
}
