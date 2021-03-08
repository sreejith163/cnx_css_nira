import { SchedulingStatus } from '../enums/scheduling-status.enum';

export class AgentScheduleBase {
    id: string;
    employeeId: number;
    firstName: string;
    lastName: string;
    status: SchedulingStatus;
    agentSchedulingGroupId: number;
}
