import { AgentScheduleRange } from './agent-schedule-range.model';

export class AgentScheduleBase {
    id: string;
    employeeId: number;
    firstName: string;
    lastName: string;
    ranges: AgentScheduleRange[];
    agentSchedulingGroupId: number;
    rangeIndex: number;
}
