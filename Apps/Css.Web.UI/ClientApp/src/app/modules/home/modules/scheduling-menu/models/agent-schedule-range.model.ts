import { SchedulingStatus } from '../enums/scheduling-status.enum';

export class AgentScheduleRange {
    dateFrom: Date;
    dateTo: Date;
    status: SchedulingStatus;
}
