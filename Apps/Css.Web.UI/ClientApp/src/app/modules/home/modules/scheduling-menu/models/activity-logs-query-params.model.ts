import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { AgentScheduleType } from '../enums/agent-schedule-type.enum';

export class ActivityLogsQueryParams extends QueryStringParameters {
    agentScheduleType: AgentScheduleType;
}
