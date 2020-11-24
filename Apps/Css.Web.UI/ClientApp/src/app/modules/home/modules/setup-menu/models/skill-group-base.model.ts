import { WeekDay } from '@angular/common';

export class SkillGroupBase {
    id: number;
    refId: number;
    name: string;
    timezoneId: number;
    timezoneLabel: string;
    firstDayOfWeek: WeekDay;
    clientId: number;
    clientName: string;
    clientLobGroupId: number;
    clientLobGroupName: string;
}
