import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class AgentChartResponse {
    id: string;
    employeeId: number;
    firstName: string;
    lastName: string;
    activeAgentShedulingGroupId: number;
    managerCharts: AgentScheduleManagerChart[];

    constructor() {
        this.managerCharts = [];
    }
}
