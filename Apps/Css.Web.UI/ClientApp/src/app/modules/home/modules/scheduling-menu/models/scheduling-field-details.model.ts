import { ActivityLogManager } from './activity-log-manager.model';
import { AgentScheduleChart } from './agent-schedule-chart.model';

export class SchedulingFieldDetails {
    activityLogRange: AgentScheduleChart[];
    activityLogManager: ActivityLogManager;
}
