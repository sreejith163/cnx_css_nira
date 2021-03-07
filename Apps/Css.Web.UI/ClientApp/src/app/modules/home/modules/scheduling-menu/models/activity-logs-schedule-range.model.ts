import { SchedulingStatus } from '../enums/scheduling-status.enum';
import { AgentScheduleChart } from './agent-schedule-chart.model';

export class ActivityLogsScheduleRange {
    agentSchedulingGroupId: number;
    dateFrom: Date;
    dateTo: Date;
    status: SchedulingStatus;
    agentScheduleCharts: AgentScheduleChart[];

    constructor() {
        this.agentScheduleCharts = [];
    }
}
