import { AgentScheduleType } from '../enums/agent-schedule-type.enum';

export class CopyAgentSchedulechart {
    employeeIds: number[];
    agentSchedulingGroupId: number;
    agentScheduleType: AgentScheduleType;
    activityOrigin: number;
    modifiedBy: string;
}
