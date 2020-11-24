import { WeekDay } from '@angular/common';

export class SkillGroupDetails {
    id: number;
    refId: number;
    clientId: number;
    clientLobGroupId: number;
    skillGroupName: string;
    clientName: string;
    clientLOBGroupName: string;
    firstDayOfWeek: WeekDay;
    timeZoneForReporting: string;
    createdBy: string;
    createdDate: string;
    modifiedBy: string;
    modifiedDate: string;
    details: SkillGroupOpenHours[] = [];
}

export class SkillGroupOpenHours {
    day: WeekDay;
    open: string;
    from: string;
    to: string;
}
