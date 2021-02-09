import { WeekDay } from "@angular/common";
import { ScheduleChart } from "../../scheduling-menu/models/schedule-chart.model";

export class AgentMyScheduleChart{
    startTime: string;
    endTime: string;
    schedulingCodeId: number;

    constructor(startTime: string, endTime: string, schedulingCodeId: number) {
        this.startTime = startTime;
        this.endTime = endTime;
        this.schedulingCodeId = schedulingCodeId;
    }
}