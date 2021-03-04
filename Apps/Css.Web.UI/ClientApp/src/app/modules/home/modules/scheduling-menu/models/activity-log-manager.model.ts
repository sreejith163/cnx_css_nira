import { ScheduleChart } from "./schedule-chart.model"

export class ActivityLogManager {
    agentSchedulingGroupId: number;
    date: Date;
    charts: ScheduleChart[];

    constructor() {
        this.charts = [];
    }
}
