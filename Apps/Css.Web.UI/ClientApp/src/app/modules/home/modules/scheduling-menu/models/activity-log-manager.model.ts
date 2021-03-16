import { ManagerScheduleChart } from './manager-schedule-chart.model';

export class ActivityLogManager {
    agentSchedulingGroupId: number;
    date: Date;
    charts: ManagerScheduleChart[];

    constructor() {
        this.charts = [];
    }
}
