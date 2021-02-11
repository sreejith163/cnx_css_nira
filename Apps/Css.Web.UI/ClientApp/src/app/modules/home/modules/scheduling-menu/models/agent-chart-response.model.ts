import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class AgentChartResponse {
    id: string;
    agentScheduleChart: AgentScheduleChart;
    agentScheduleManagerCharts: AgentScheduleManagerChart[];

    constructor() {
        this.agentScheduleManagerCharts = [];
    }
}
