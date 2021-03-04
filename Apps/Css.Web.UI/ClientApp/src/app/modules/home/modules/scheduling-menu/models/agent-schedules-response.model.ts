import { AgentScheduleBase } from './agent-schedule-base.model';
import { AgentScheduleRange } from './agent-schedule-range.model';

export class AgentSchedulesResponse extends AgentScheduleBase {
    ranges: AgentScheduleRange[];
    rangeIndex: number;
    createdBy: string;
    createdDate: Date;
    modifiedBy: string;
    modifiedDate: Date;
}
