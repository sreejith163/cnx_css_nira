import { WeekDay } from '@angular/common';
import {AgentMyScheduleChart} from '../models/agent-myschedule-chart.model';

export class AgentMyScheduleDetails{

    day: WeekDay;
    date: Date;
    firsStartTime: string;
    lastEndTime: string;
    charts: AgentMyScheduleChart[];

    constructor() {
        this.charts = [];
    }

}