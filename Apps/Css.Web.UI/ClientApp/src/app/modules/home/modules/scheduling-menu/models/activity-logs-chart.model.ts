import { ActivityLogsBase } from './activity-logs-base.model';
import { AgentScheduleChart } from './agent-schedule-chart.model';

export class ActivityLogsChart extends ActivityLogsBase {
    // employeeId: number;
    id: number;
    agentScheduleChart: AgentScheduleChart;
}
