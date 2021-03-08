import { SchedulingStatus } from '../enums/scheduling-status.enum';
import { AgentScheduleChart } from './agent-schedule-chart.model';

export class UpdateAgentschedulechart {
    agentScheduleCharts: AgentScheduleChart[];
    activityOrigin: number;
    dateFrom: Date;
    dateTo: Date;
    status: SchedulingStatus;
    modifiedUser: number;
    modifiedBy: string;
}
