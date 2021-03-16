export class ManagerScheduleChart {
    startDateTime: Date;
    endDateTime: Date;
    schedulingCodeId: number;

    constructor(startTime: Date, endTime: Date, schedulingCodeId: number) {
        this.startDateTime = startTime;
        this.endDateTime = endTime;
        this.schedulingCodeId = schedulingCodeId;
    }
}