import { WeekDay } from '@angular/common';

export class ActivityLogsBase {
    id: string;
    employeeId: number;
    day: WeekDay;
    timeStamp: Date;
    executedBy: string;
    origin: string;
    status: string;
}
