import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { AgentScheduleType } from '../enums/agent-schedule-type.enum';

export class AgentSchedulesQueryParams extends QueryStringParameters {
    agentSchedulingGroupId?: number;
    fromDate?: string;
    employeeIds: number[];
    skipPageSize: boolean;
}
