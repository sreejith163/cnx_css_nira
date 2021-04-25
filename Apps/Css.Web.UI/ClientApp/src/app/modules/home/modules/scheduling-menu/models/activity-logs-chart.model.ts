import { ActivityLogsBase } from './activity-logs-base.model';
import { AgentScheduleChart } from './agent-schedule-chart.model';

export class ActivityLogsChart extends ActivityLogsBase {
    // employeeId: string;
    id: number;
    agentScheduleChart: AgentScheduleChart;
}
