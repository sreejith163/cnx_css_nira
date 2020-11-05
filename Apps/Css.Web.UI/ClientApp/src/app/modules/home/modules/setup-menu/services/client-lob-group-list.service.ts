import { Injectable } from '@angular/core';
import { DaysOfWeek } from 'src/app/shared/enums/days-of-week.enum';
import { ClientLOBGroupDetails } from '../models/client-lob-group-details.model';
import { TimeZone } from '../models/time-zone.model';

@Injectable()
export class ClientLobGroupListService {
  clientLobGroup: ClientLOBGroupDetails[] = [];
  firstDayOfWeek = DaysOfWeek;
  timeZoneValues = [
    'Atlantic Standard Time (AST)',
    'Eastern Standard Time (EST)',
    'Central Standard Time (CST)',
    'Mountain Standard Time (MST)',
    'Pacific Standard Time (PST)',
    'Alaskan Standard Time (AKST)',
    'Hawaii-Aleutian Standard Time (HST)',
    'Samoa standard time (UTC-11)',
    'Chamorro Standard Time (UTC+10)',
  ];
  timeZone: TimeZone[] = [];

  constructor() {
    this.createTimeZoneList();
    this.createClientLOBGroup();
  }

  createTimeZoneList() {
    for (let i = 1; i <= 7; i++) {
      const timeZone = new TimeZone();
      timeZone.id = i;
      timeZone.timeZoneName = this.timeZoneValues[i];
      this.timeZone.push(timeZone);
    }
  }

  getTimeZoneList() {
    return this.timeZone;
  }

  createClientLOBGroup() {
    for (let i = 1; i <= 7; i++) {
      const lobGroup = new ClientLOBGroupDetails();
      lobGroup.id = i;
      lobGroup.refId = i;
      lobGroup.clientName = 'Client ' + i;
      lobGroup.firstDayOfWeek = i - 1;
      //lobGroup.clientLOBGroupName = 'ClientLOBGroup ' + i;
      const timeZone = this.timeZone.find(x => x.id === i);
      lobGroup.timeZoneForReporting = timeZone.timeZoneName;
      lobGroup.createdDate = '2020-09-1' + i;
      lobGroup.createdBy = 'User ' + i;
      lobGroup.modifiedDate = '2020-09-1' + i;
      lobGroup.modifiedBy = 'User ' + i;

      this.clientLobGroup.push(lobGroup);
    }
  }

  addClientLOBGroup(lobGroup: ClientLOBGroupDetails) {
    lobGroup.id = this.clientLobGroup.length + 1;
    lobGroup.refId = this.clientLobGroup.length + 1;
    lobGroup.createdDate = String(new Date());
    lobGroup.createdBy = 'User ' + (this.clientLobGroup.length + 1);
    this.clientLobGroup.push(lobGroup);
  }

  updateClientLOBGroup(lobGroup: ClientLOBGroupDetails) {
    lobGroup.modifiedDate = String(new Date());
    lobGroup.modifiedBy = 'User ' + (this.clientLobGroup.length + 1);
    const lobGroupIndex = this.clientLobGroup.findIndex(
      (x) => x.id === lobGroup.id
    );
    if (lobGroupIndex !== -1) {
      this.clientLobGroup[lobGroupIndex] = lobGroup;
    }
  }

  deleteClientLOBGroup(lobGroupId: number) {
    const lobGroupIndex = this.clientLobGroup.findIndex(
      (x) => x.id === lobGroupId
    );
    if (lobGroupIndex !== -1) {
      this.clientLobGroup.splice(lobGroupIndex, 1);
    }
  }

  getClientLOBGroups() {
    return this.clientLobGroup;
  }

  getClientLOBGroupById(lobGroupId) {
    return this.clientLobGroup.filter((x) => x.id === lobGroupId);
  }
}
