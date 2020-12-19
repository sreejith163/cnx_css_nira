import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class AgentSchedulesQueryParams extends QueryStringParameters {
    agentSchedulingGroupId: number;
    fromDate: Date;
}
