import { SchedulingStatus } from '../enums/scheduling-status.enum';
import { AgentScheduleChart } from './agent-schedule-chart.model';

export class AgentScheduleRange {
    agentSchedulingGroupId: number;
    dateFrom: string;
    dateTo: string;
    status: SchedulingStatus;
    scheduleCharts: AgentScheduleChart[];
    createdBy: string;
    createdDate: Date;
    modifiedBy: string;
    modifiedDate: Date;

    constructor() {
        this.scheduleCharts = [];
    }
}
