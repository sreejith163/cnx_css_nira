import { AgentScheduleChart } from './agent-schedule-chart.model';

export class ImportAgentScheduleRanges {
    dateFrom: Date;
    dateTo: Date;
    agentScheduleCharts: AgentScheduleChart[];

    constructor() {
        this.agentScheduleCharts = [];
    }
}
