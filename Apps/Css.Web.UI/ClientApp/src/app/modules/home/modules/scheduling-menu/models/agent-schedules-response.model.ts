import { AgentScheduleBase } from './agent-schedule-base.model';

export class AgentSchedulesResponse extends AgentScheduleBase {
    createdBy: string;
    createdDate: Date;
    modifiedBy: string;
    modifiedDate: Date;
}
