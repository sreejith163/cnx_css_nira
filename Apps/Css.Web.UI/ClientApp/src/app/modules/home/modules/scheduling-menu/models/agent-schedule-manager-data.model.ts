import { ScheduleChart } from './schedule-chart.model';

export class AgentShceduleMangerData {
    employeeId: number;
    charts: ScheduleChart[];

    constructor() {
        this.charts = [];
    }
}
