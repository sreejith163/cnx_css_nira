import { SchedulingStatus } from '../enums/scheduling-status.enum';
import { AgentScheduleChart } from './agent-schedule-chart.model';

export class UpdateAgentschedulechart {
    agentScheduleCharts: AgentScheduleChart[];
    activityOrigin: number;
    dateFrom: string;
    dateTo: string;
    status: SchedulingStatus;
    modifiedUser: number;
    modifiedBy: string;
}
