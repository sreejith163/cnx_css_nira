import { SchedulingStatus } from '../enums/scheduling-status.enum';
import { AgentScheduleBase } from './agent-schedule-base.model';
import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class AgentScheduleGridResponse extends AgentScheduleBase {
    agentScheduleCharts: AgentScheduleChart[];
    agentScheduleManagerCharts: AgentScheduleManagerChart[];
    dateFrom: Date;
    dateTo: Date;
}
