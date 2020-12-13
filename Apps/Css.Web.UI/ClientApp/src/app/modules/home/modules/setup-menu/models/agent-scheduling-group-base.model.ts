import { WeekDay } from '@angular/common';

export class AgentSchedulingGroupBase {
    id: number;
    refId: number;
    name: string;
    clientId: number;
    clientName: string;
    clientLobGroupId: number;
    clientLobGroupName: string;
    skillGroupId: number;
    skillGroupName: string;
    skillTagId: number;
    skillTagName: string;
    timezoneId: number;
    timezoneLabel: string;
    firstDayOfWeek: WeekDay;
}
