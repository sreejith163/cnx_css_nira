import { WeekDay } from '@angular/common';
import { SchedulingCalendarTime } from './scheduling-calendar-times.model';

export class SchedulingCalendarDays {
    day: WeekDay;
    times: SchedulingCalendarTime[];

    constructor() {
        this.times = [];
    }
}
