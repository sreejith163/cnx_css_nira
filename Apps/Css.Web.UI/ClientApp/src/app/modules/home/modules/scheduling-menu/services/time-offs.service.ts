import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { AddTimeOffs } from '../models/add-time-offs.model';
import { TimeOffResponse } from '../models/time-offs-response.model';

@Injectable()
export class TimeOffsService {

  timeOffs: TimeOffResponse[] = [];

  constructor() {
    this.createTimeOffs();
  }

  createTimeOffs() {
    for (let i = 1; i <= 7; i++) {
      const timeOff = new TimeOffResponse();
      timeOff.id = i;
      timeOff.description = 'Description' + i;
      timeOff.startDate = new Date('01/01/2021');
      timeOff.endDate = new Date('10/01/2021');
      timeOff.allowDayRequestOn = i < 5 ? [i, i + 1, i + 2] : [1, 2, 3];
      timeOff.firstDayOfWeek = i - 1;
      timeOff.timeOffCode = i;
      timeOff.FTEDayLength = '0' + i + ':' + '00';
      timeOff.agentAccess = {
        addNoteAllotments: true,
        waitList: true,
        timeOffAnyDay: true,
        timeOffAllotments: true,
        showPastDays: true
      };
      timeOff.fullWeeks = {
        daysAfterWeek: 22,
        daysBeforeWeek: 99,
        fullWeekList: [1, 2, 3]
      };
      this.timeOffs.push(timeOff);
    }
  }

  getTimeOffs() {
    return of(this.timeOffs);
  }

  addTimeOff(data: TimeOffResponse) {
    data.id = this.timeOffs.length + 1;
    this.timeOffs.push(data);
    return of(undefined);
  }

  updateTimeOff(data: TimeOffResponse) {
    const index = this.timeOffs.findIndex(x => x.id === data.id);
    if (index > -1) {
      this.timeOffs[index] = data;
    }
    return of(undefined);
  }

  deleteTimeoff(id: number) {
    const index = this.timeOffs.findIndex(x => x.id === id);
    if (index > -1) {
      this.timeOffs.splice(index, 1);
    }
    return of(undefined);
  }

}
