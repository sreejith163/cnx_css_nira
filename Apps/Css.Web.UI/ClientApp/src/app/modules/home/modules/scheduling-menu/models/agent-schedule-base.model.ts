import { SchedulingStatus } from '../enums/scheduling-status.enum';

export class AgentScheduleBase {
    id: string;
    employeeId: string;
    employeeName: string;
    status: SchedulingStatus;
    agentSchedulingGroupId: number;
}
