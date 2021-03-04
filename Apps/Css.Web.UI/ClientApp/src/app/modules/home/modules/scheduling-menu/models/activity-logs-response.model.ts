import { ActivityLogsBase } from './activity-logs-base.model';
import { SchedulingFieldDetails } from './scheduling-field-details.model';

export class ActivityLogsResponse extends ActivityLogsBase {
    id: string;
    schedulingFieldDetails: SchedulingFieldDetails;
    fieldDetails: FieldData [];
}

export class FieldData{
    name: string;
    oldValue: string;
    newValue: string;
}
