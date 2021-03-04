import { ScheduleChart } from './schedule-chart.model';

export class AgentChartResponse {
    id: string;
    employeeId: number;
    firstName: string;
    lastName: string;
    activeAgentShedulingGroupId: number;
    date: Date;
    charts: ScheduleChart[];

    constructor() {
        this.charts = [];
    }
}
