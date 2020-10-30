import { WeekDay } from '@angular/common';
import { Injectable } from '@angular/core';
import { DaysOfWeek } from 'src/app/shared/enums/days-of-week.enum';
import {
  SkillGroupDetails,
  SkillGroupOpenHours,
} from '../models/skill-group.model';
import { TimeZone } from '../models/time-zone.model';

@Injectable()
export class SkillGroupsService {
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
  skillGroups: SkillGroupDetails[] = [];

  constructor() {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.createTimeZoneList();
    this.createSkillGroup();
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

  createSkillGroup() {
    for (let i = 1; i <= 7; i++) {
      const skillGroup = new SkillGroupDetails();
      skillGroup.id = i;
      skillGroup.refId = i;
      skillGroup.skillGroupName = 'Skill Group ' + i;
      skillGroup.clientName = 'Client ' + i;
      skillGroup.firstDayOfWeek = i - 1;
      skillGroup.clientLOBGroupName = 'ClientLOBGroup ' + i;
      const timeZone = this.timeZone.find((x) => x.id === i);
      skillGroup.timeZoneForReporting = timeZone.timeZoneName;
      skillGroup.createdDate = '2020-09-1' + i;
      skillGroup.createdBy = 'User ' + i;
      skillGroup.modifiedDate = '2020-09-1' + i;
      skillGroup.modifiedBy = 'User ' + i;
      skillGroup.details = this.createSkillGroupOpenHoursDetails(skillGroup.firstDayOfWeek);

      this.skillGroups.push(skillGroup);
    }
  }

  addSkillGroup(skillGroup: SkillGroupDetails) {
    skillGroup.id = this.skillGroups.length + 1;
    skillGroup.refId = this.skillGroups.length + 1;
    skillGroup.createdBy = 'User';
    skillGroup.createdDate = String(new Date());
    this.skillGroups.push(skillGroup);
  }

  updateSkillGroup(skillGroup: SkillGroupDetails) {
    skillGroup.modifiedDate = String(new Date());
    skillGroup.modifiedBy = skillGroup.modifiedBy
      ? skillGroup.modifiedBy
      : 'User';
    const skillGroupIndex = this.skillGroups.findIndex(
      (x) => x.id === skillGroup.id
    );
    if (skillGroupIndex !== -1) {
      this.skillGroups[skillGroupIndex] = skillGroup;
    }
  }

  deleteSkillGroup(skillGroupId: number) {
    const skillGroupIndex = this.skillGroups.findIndex(
      (x) => x.id === skillGroupId
    );
    if (skillGroupIndex !== -1) {
      this.skillGroups.splice(skillGroupIndex, 1);
    }
  }

  getSkillGroups() {
    return this.skillGroups;
  }

  getSkillGroupById(skillGroupId) {
    return this.skillGroups.filter((x) => x.id === skillGroupId);
  }

  createSkillGroupOpenHoursDetails(firstDayOfWeek: WeekDay) {
    this.weekDays = this.getSortedWeekDays(firstDayOfWeek);

    const skillGroupDetails: SkillGroupOpenHours[] = [];
    this.weekDays.forEach((element, index) => {
      const skillGroupOpenhours = new SkillGroupOpenHours();
      skillGroupOpenhours.day = element;
      skillGroupOpenhours.open =
      index % 2 === 1 ? 'Open Partial Day' : 'Open All Day';
      skillGroupOpenhours.from = index % 2 === 1 ? '06:00 am' : '';
      skillGroupOpenhours.to = index % 2 === 1 ? '05:00 pm' : '';
      skillGroupDetails.push(skillGroupOpenhours);
    });

    return skillGroupDetails;
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
