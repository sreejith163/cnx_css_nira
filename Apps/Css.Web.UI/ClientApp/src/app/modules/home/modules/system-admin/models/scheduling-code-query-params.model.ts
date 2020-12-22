import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class SchedulingCodeQueryParams extends QueryStringParameters {
    skipPageSize: boolean;
    activityCodes: string[];
}
