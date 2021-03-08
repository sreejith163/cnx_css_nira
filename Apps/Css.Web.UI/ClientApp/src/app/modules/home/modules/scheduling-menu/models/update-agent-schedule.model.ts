import { SchedulingStatus } from '../enums/scheduling-status.enum';

export class UpdateAgentSchedule {
    status: SchedulingStatus;
    dateFrom: Date;
    dateTo: Date;
    modifiedBy: string;
    modifiedUser: number;
    activityOrigin: number;
}
