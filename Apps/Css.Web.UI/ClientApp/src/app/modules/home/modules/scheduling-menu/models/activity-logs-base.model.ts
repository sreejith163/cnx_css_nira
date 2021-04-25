import { WeekDay } from '@angular/common';
import { ActivityOrigin } from '../enums/activity-origin.enum';
import { ActivityStatus } from '../enums/activity-status.enum';

export class ActivityLogsBase {
    employeeId: string;
    executedUser: string;
    day: WeekDay;
    timeStamp: Date;
    executedBy: string;
    origin: string;
    activityStatus: ActivityStatus;
    activityOrigin: ActivityOrigin;
}
