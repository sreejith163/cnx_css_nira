import { Injectable } from '@angular/core';
import { SkillTag, SkillTagOpenHours } from '../models/skill-tag.model';
import { of } from 'rxjs';

@Injectable()

export class SkillTagsService {
  skillTags: SkillTag[] = [];
  skillGroup: Array<any> = [];
  constructor() {
    this.createSkillTagsList();
    this.createSkillGroupList();
  }

  createSkillGroupList() {
    for (let i = 1; i <= 9; i++) {
      const skillGroup = {
        id: i,
        skillGroup: `Skill Group ${i}`
      };
      this.skillGroup.push(skillGroup);
    }
  }

  createSkillTagsList() {
    for (let i = 1; i <= 9; i++) {
      const skillTag = new SkillTag();
      skillTag.id = i;
      skillTag.refId = i;
      skillTag.skillTagName = 'Name ' + i;
      skillTag.createdDate = '2020-09-1' + i;
      skillTag.createdBy = 'User ' + i;
      skillTag.modifiedDate = '2020-09-1' + i;
      skillTag.modifiedBy = 'User ' + i;
      skillTag.clientName = 'Client ' + i;
      skillTag.clientLobGroup = 'ClientLOBName ' + i;
      skillTag.skillGroup =  'Skill Group ' + i;
      skillTag.details = this.createOpenHoursDetails();

      this.skillTags.push(skillTag);
    }
  }

  createOpenHoursDetails() {
    const skillTagDetails: SkillTagOpenHours[] = [];
    for (let i = 0; i < 7; i++) {
      const skillTagOpenhours = new SkillTagOpenHours();
      skillTagOpenhours.day = i;
      skillTagOpenhours.open = ((i % 2) === 1) ? 'Open Partial Day' : 'Open All Day';
      skillTagOpenhours.from = ((i % 2) === 1) ? '06:00 am' : '';
      skillTagOpenhours.to = ((i % 2) === 1) ? '05:00 pm' : '';
      skillTagDetails.push(skillTagOpenhours);
    }

    return skillTagDetails;
  }

  getSkillTags() {
    return of(this.skillTags);
  }

  getSkillGroupList() {
    return this.skillGroup;
  }

  deleteSkillTag(id: number) {
    const index = this.skillTags.findIndex(x => x.id === id);
    if (index !== -1) {
      this.skillTags.splice(index, 1);
    }
  }

  updateSkillTagDetials(details: SkillTag) {
    details.modifiedDate = String(new Date());
    details.modifiedBy = 'User';
    const clientIndex = this.skillTags.findIndex(x => x.id === details.id);
    if (clientIndex !== -1) {
      this.skillTags[clientIndex] = details;
    }
  }

  addSkillTagDetails(details: SkillTag) {
    details.id = this.skillTags.length + 1;
    details.refId = this.skillTags.length + 1;
    details.createdBy = 'User';
    details.createdDate = String(new Date());
    this.skillTags.push(details);
  }

  openOptions() {
    return [
      {
        id: 0,
        open: 'Open All Day'
      },
      {
        id: 1,
        open: 'Open Partial Day'
      },
      {
        id: 2,
        open: 'Closed'
      }
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
      const mm = (tt % 60); // getting minutes of the hour in 0-55 format
      // pushing data in array in [00.00 - 12.00 am/pm format]
      times[i] = ('0' + (hh % 12)).slice(-2) + ':' + ('0' + mm).slice(-2) + ' ' + ap[Math.floor(hh / 12)];
      tt = tt + x;
    }
    return times;
  }

  getHours(time: string) {
    const timeArray = time.split(':');
    if (timeArray[1].split(' ')[1] === 'pm') {
      return (parseInt(timeArray[0], 10) + 12);
    } else {
      return parseInt(timeArray[0], 10);
    }
  }

  getMinutes(time: string) {
    return +time.split(':')[1].split(' ')[0];
  }
}
