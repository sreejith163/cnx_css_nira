import { ActivityLogsBase } from './activity-logs-base.model';
import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class ActivityLogsResponse extends ActivityLogsBase {
    agentScheduleCharts: AgentScheduleChart[];
    agentScheduleManagerCharts: AgentScheduleManagerChart[];
}
