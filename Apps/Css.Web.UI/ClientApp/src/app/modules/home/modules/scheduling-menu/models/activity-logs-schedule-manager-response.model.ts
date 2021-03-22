import { ActivityLogScheduleManager } from './activity-log-schedule-manager.model';
import { ActivityLogsBase } from './activity-logs-base.model';
import { SchedulingFieldDetails } from './scheduling-field-details.model';

export class ActivityLogsScheduleManagerResponse extends ActivityLogsBase {
    id: string;
    schedulingFieldDetails: SchedulingManagerFieldDetails;
    fieldDetails: FieldData [];
}

export class FieldData{
    name: string;
    oldValue: string;
    newValue: string;
}

export class SchedulingManagerFieldDetails {
    activityLogManager: ActivityLogScheduleManager;
}