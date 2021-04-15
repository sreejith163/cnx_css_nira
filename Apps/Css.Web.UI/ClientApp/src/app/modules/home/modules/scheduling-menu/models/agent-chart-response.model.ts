import { ScheduleChart } from './schedule-chart.model';

export class AgentChartResponse {
    id: string;
    employeeId: string;
    firstName: string;
    lastName: string;
    agentSchedulingGroupId: number;
    date: Date;
    charts: ScheduleChart[];

    constructor() {
        this.charts = [];
    }
}
