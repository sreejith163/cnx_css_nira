import { WeekDay } from '@angular/common';

export class SchedulingGroupDetails {
    id: number;
    refId: number;
    schedulingGroupName: string;
    clientName: string;
    clientLOBGroupName: string;
    skillGroup: string;
    skillTag: string;
    firstDayOfWeek: WeekDay;
    timeZoneForReporting: string;
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;
    details: SchedulingGroupOpenHours[];
}

export class SchedulingGroupOpenHours {
    day: WeekDay;
    open: string;
    from: string;
    to: string;
}
