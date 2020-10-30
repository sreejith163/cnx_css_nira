import { WeekDay } from '@angular/common';

export class ClientLOBGroupDetails {
    id: number;
    refId: number;
    clientName: string;
    clientLOBGroupName: string;
    firstDayOfWeek: WeekDay;
    timeZoneForReporting: string;
    createdDate: string;
    createdBy: string;
    modifiedDate: string;
    modifiedBy: string;
}
