import { ClientLobGroupBase } from './client-lob-group-base.model';

export class UpdateClientLobGroup extends ClientLobGroupBase {
  ModifiedBy: string;
  clientName: string;
  firstDayOfWeek: string;
  timeZoneForReporting: string;
  timeZoneId: number;
  clientId: number;
}
