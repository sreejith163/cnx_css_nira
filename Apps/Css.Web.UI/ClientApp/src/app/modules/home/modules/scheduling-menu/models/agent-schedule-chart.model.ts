import { WeekDay } from '@angular/common';
import { ScheduleChart } from './schedule-chart.model';

export class AgentScheduleChart {
    day: WeekDay;
    charts: ScheduleChart[];

    constructor() {
        this.charts = [];
    }
}
