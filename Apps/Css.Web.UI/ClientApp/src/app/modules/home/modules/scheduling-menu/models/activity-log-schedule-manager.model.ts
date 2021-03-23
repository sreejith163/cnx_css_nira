import { WeekDay } from "@angular/common";
import { ScheduleChart } from "./schedule-chart.model"
import { ScheduleManagerChart } from "./schedule-manager-chart.model";
import { ActivityLogsBase } from './activity-logs-base.model';

export class ActivityLogScheduleManager {
    agentSchedulingGroupId: number;
    date: Date;
    charts: ScheduleManagerChart[];

    constructor() {
        this.charts = [];
    }
}

export class ActivityLogsManagerChart extends ActivityLogsBase {
    // employeeId: number;
    id: number;
    agentScheduleChart: AgentScheduleManagerChart;
}

export class AgentScheduleManagerChart {
    day: WeekDay;
    charts: ScheduleManagerChart[];

    constructor() {
        this.charts = [];
    }
}
