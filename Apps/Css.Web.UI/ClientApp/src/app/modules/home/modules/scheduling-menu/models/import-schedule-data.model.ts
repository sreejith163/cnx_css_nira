import { AgentScheduleChart } from './agent-schedule-chart.model';

export class ImportScheduleData {
    employeeId: number;
    dateFrom: Date;
    dateTo: Date;
    agentScheduleCharts: AgentScheduleChart[];

    constructor() {
        this.agentScheduleCharts = [];
    }
}
