import { SchedulingStatus } from '../enums/scheduling-status.enum';
import { SchedulingCalendar } from './scheduling-calendar.model';

export class SchedulingGrid {
    employeeId: string;
    name: string;
    fromDate: string;
    toDate: string;
    status: SchedulingStatus;
    calendar: SchedulingCalendar;
    createdDate: string;
    createdBy: string;
    modifiedBy: string;
    modifiedDate: string;

    constructor() {
        this.calendar = new SchedulingCalendar();
    }
}
