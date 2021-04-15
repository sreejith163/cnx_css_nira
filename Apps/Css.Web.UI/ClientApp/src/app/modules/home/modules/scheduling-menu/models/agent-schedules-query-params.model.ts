import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { SchedulingStatus } from '../enums/scheduling-status.enum';

export class AgentSchedulesQueryParams extends QueryStringParameters {
    agentSchedulingGroupId?: number;
    status?: SchedulingStatus;
    dateFrom?: string;
    dateTo?: string;
    excludeConflictSchedule: boolean;
    employeeIds: string[];
    excludedEmployeeId?: string;
}
