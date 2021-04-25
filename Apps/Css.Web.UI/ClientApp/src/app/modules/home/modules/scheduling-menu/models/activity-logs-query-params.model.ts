import { ActivityType } from 'src/app/shared/enums/activity-type.enum';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class ActivityLogsQueryParams extends QueryStringParameters {
    activityType: ActivityType;
    skipPageSize: boolean;
    employeeId: string;
    dateFrom: string;
    dateto: string;
    date: string;
}
