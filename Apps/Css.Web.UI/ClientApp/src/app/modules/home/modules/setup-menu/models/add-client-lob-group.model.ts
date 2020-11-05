import { ClientLobGroupBase } from './client-lob-group-base.model';

export class AddClientLobGroup extends ClientLobGroupBase {
    createdBy: string;
    firstDayOfWeek: number;
    timeZoneId: number;
    clientId: number;
}

