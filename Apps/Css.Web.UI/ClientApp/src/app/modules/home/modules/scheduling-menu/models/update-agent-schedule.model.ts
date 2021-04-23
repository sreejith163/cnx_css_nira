import { SchedulingStatus } from '../enums/scheduling-status.enum';

export class UpdateAgentSchedule {
    status: SchedulingStatus;
    dateFrom: string;
    dateTo: string;
    modifiedBy: string;
    modifiedUser: number;
    activityOrigin: number;
}
