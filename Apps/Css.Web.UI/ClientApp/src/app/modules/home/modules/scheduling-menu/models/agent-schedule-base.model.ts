import { AgentScheduleRange } from './agent-schedule-range.model';

export class AgentScheduleBase {
    id: string;
    employeeId: string;
    firstName: string;
    lastName: string;
    activeAgentSchedulingGroupId: number;
}
