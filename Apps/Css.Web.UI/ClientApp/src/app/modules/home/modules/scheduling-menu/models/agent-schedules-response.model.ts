import { AgentScheduleBase } from './agent-schedule-base.model';

export class AgentSchedulesResponse extends AgentScheduleBase {
    dateFrom: Date;
    dateTo: Date;
    createdBy: string;
    createdDate: Date;
    modifiedBy: string;
    modifiedDate: Date;
}
