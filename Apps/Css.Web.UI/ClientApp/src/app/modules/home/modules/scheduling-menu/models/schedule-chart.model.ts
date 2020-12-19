export class ScheduleChart {
    startTime: string;
    endTime: string;
    schedulingCodeId: number;

    constructor(startTime: string, endTime: string, schedulingCodeId: number) {
        this.startTime = startTime;
        this.endTime = endTime;
        this.schedulingCodeId = schedulingCodeId;
    }
}
