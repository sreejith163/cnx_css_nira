import { SchedulingStatus } from "../enums/scheduling-status.enum";
import { AgentScheduleChart } from "./agent-schedule-chart.model";

export class ActivityLogRange {
    agentSchedulingGroupId: number;
    dateFrom: Date;
    dateTo: Date;
    status: SchedulingStatus;
    scheduleCharts: AgentScheduleChart[];
}
