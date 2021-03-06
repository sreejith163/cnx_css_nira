import { WeekDay } from '@angular/common';
import { ClientLobGroupBase } from './client-lob-group-base.model';

export class ClientLOBGroupDetails extends ClientLobGroupBase {
    clientId: number;
    clientName: string;
    timezoneId: number;
    timezoneLabel: string;
    firstDayOfWeek: WeekDay;
    timeZoneForReporting: string;
    createdDate: string;
    createdBy: string;
    modifiedDate: string;
    modifiedBy: string;
}
