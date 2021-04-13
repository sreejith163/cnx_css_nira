import { WeekDay } from "@angular/common";
import { ScheduleChart } from "../../scheduling-menu/models/schedule-chart.model";

export class AgentMyScheduleChart{
    startDateTime: string;
    endDateTime: string;
    schedulingCodeId: number;

    constructor(startTime: string, endTime: string, schedulingCodeId: number) {
        this.startDateTime = startTime;
        this.endDateTime = endTime;
        this.schedulingCodeId = schedulingCodeId;
    }
}