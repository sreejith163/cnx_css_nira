import { WeekDay } from '@angular/common';

export class ActivityLogsBase {
    id: string;
    employeeId: number;
    day: WeekDay;
    timeStamp: string;
    executedBy: string;
    origin: string;
    status: string;
}
