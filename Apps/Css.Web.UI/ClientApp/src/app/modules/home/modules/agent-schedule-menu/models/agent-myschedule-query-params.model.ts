import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class AgentMyScheduleQueryParams extends QueryStringParameters {
    agentSchedulingGroupId?: number;
    fromDate?: string;
    employeeIds: string[];
}
