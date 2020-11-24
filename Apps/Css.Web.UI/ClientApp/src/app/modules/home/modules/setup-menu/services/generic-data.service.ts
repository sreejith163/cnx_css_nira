import { Injectable } from '@angular/core';

@Injectable()
export class GenericDataService {

  constructor() { }

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
