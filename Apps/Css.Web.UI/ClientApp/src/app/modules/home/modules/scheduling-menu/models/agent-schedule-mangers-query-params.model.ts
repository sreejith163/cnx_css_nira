import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class AgentScheduleManagersQueryParams extends QueryStringParameters {
    employeeId: number;
    agentSchedulingGroupId: number;
    excludeConflictSchedule: boolean;
    date: string;
}
