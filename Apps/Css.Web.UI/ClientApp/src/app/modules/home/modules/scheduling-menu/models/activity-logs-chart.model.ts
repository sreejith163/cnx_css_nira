import { ActivityLogsBase } from './activity-logs-base.model';
import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';
import { ManagerScheduleChart } from './manager-schedule-chart.model';

export class ActivityLogsChart extends ActivityLogsBase {
    // employeeId: number;
    id: number;
    agentScheduleChart: AgentScheduleChart;
    managerChart: AgentScheduleManagerChart[];

    constructor() {
        super();
        this.managerChart = [];
    }
}
