import { WeekDay } from '@angular/common';
import { Injectable } from '@angular/core';
import { DaysOfWeek } from 'src/app/shared/enums/days-of-week.enum';
import { TimeZone } from 'src/app/shared/models/time-zone.model';
import {
  SchedulingGroupDetails,
  SchedulingGroupOpenHours,
} from '../models/scheduling-group-details.model';

@Injectable()
export class AgentSchedulingGroupService {

  firstDayOfWeek = DaysOfWeek;
  weekDays: Array<WeekDay>;
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
  schedulingGroups: SchedulingGroupDetails[] = [];

  constructor() {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.createTimeZoneList();
    this.createSchedulingGroup();
  }

  createTimeZoneList() {
    for (let i = 1; i <= 7; i++) {
      const timeZone = new TimeZone();
      timeZone.id = i;
      timeZone.value = this.timeZoneValues[i];
      this.timeZone.push(timeZone);
    }
  }

  getTimeZoneList() {
    return this.timeZone;
  }

  createSchedulingGroup() {
    for (let i = 1; i <= 7; i++) {
      const schedulingGroup = new SchedulingGroupDetails();
      schedulingGroup.id = i;
      schedulingGroup.refId = i;
      schedulingGroup.schedulingGroupName = 'Scheduling Group ' + i;
      schedulingGroup.clientName = 'Client ' + i;
      schedulingGroup.skillTag = 'Skill Tag ' + i;
      schedulingGroup.skillGroup = 'Skill Group ' + i;
      schedulingGroup.firstDayOfWeek = i - 1;
      schedulingGroup.clientLOBGroupName = 'ClientLOBName ' + i;
      const timeZone = this.timeZone.find((x) => x.id === i);
      schedulingGroup.timeZoneForReporting = timeZone.value;
      schedulingGroup.createdDate = '2020-09-1' + i;
      schedulingGroup.createdBy = 'User ' + i;
      schedulingGroup.modifiedDate = '2020-09-1' + i;
      schedulingGroup.modifiedBy = 'User ' + i;
      schedulingGroup.details = this.createScheduledGroupOpenHoursDetails(schedulingGroup.firstDayOfWeek);

      this.schedulingGroups.push(schedulingGroup);
    }
  }

  addSchedulingGroup(schedulingGroup: SchedulingGroupDetails) {
    schedulingGroup.id = this.schedulingGroups.length + 1;
    schedulingGroup.refId = this.schedulingGroups.length + 1;
    schedulingGroup.createdDate = String(new Date());
    schedulingGroup.createdBy = 'User';
    this.schedulingGroups.push(schedulingGroup);
  }

  updateSchedulingGroup(schedulingGroup: SchedulingGroupDetails) {
    schedulingGroup.modifiedDate = String(new Date());
    schedulingGroup.modifiedBy = schedulingGroup.modifiedBy
      ? schedulingGroup.modifiedBy
      : 'User';
    const schedulingGroupIndex = this.schedulingGroups.findIndex(
      (x) => x.id === schedulingGroup.id
    );
    if (schedulingGroupIndex !== -1) {
      this.schedulingGroups[schedulingGroupIndex] = schedulingGroup;
    }
  }

  deleteSchedulingGroup(schedulingGroupId: number) {
    const schedulingGroupIndex = this.schedulingGroups.findIndex(
      (x) => x.id === schedulingGroupId
    );
    if (schedulingGroupIndex !== -1) {
      this.schedulingGroups.splice(schedulingGroupIndex, 1);
    }
  }

  getSchedulingGroups() {
    return this.schedulingGroups;
  }

  getSchedulingGroupById(schedulingGroupId) {
    return this.schedulingGroups.filter((x) => x.id === schedulingGroupId);
  }

  createScheduledGroupOpenHoursDetails(firstDayOfWeek: WeekDay) {
    this.weekDays = this.getSortedWeekDays(firstDayOfWeek);

    const schedulingGroupDetails: SchedulingGroupOpenHours[] = [];
    this.weekDays.forEach((element, index) => {
      const schedulingGroupOpenhours = new SchedulingGroupOpenHours();
      schedulingGroupOpenhours.day = element;
      schedulingGroupOpenhours.open =
      index % 2 === 1 ? 'Open Partial Day' : 'Open All Day';
      schedulingGroupOpenhours.from = index % 2 === 1 ? '06:00 am' : '';
      schedulingGroupOpenhours.to = index % 2 === 1 ? '05:00 pm' : '';
      schedulingGroupDetails.push(schedulingGroupOpenhours);
    });

    return schedulingGroupDetails;
  }

  openOptions() {
    return [
      {
        id: 0,
        open: 'Open All Day',
      },
      {
        id: 1,
        open: 'Open Partial Day',
      },
      {
        id: 2,
        open: 'Closed',
      },
    ];
  }

  openTimes() {
    const x = 15; // minutes interval
    const times = []; // time array
    let tt = 0; // start time
    const ap = ['am', 'pm']; // AM-PM

    // loop to increment the time and push results in array
    for (let i = 0; tt < 24 * 60; i++) {
      const hh = Math.floor(tt / 60); // getting hours of day in 0-24 format
      const mm = tt % 60; // getting minutes of the hour in 0-55 format
      times[i] =
        ('0' + (hh % 12)).slice(-2) +
        ':' +
        ('0' + mm).slice(-2) +
        ' ' +
        ap[Math.floor(hh / 12)]; // pushing data in array in [00.00 - 12.00 am/pm format]
      tt = tt + x;
    }
    return times;
  }

  private getSortedWeekDays(day: number) {
    const days = [];
    const previousDays = [];
    let found = false;

    this.weekDays.forEach((e) => {
      if (e === day && !found) {
        found = true;
        days.push(e);
      } else if (!found) {
        previousDays.push(e);
      } else {
        days.push(e);
      }
    });

    return (days.concat(previousDays));
  }
}
