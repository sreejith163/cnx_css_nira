import { ActivityLogsBase } from './activity-logs-base.model';
import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';
import { SchedulingFieldDetails } from './scheduling-field-details.model';

export class ActivityLogsResponse extends ActivityLogsBase {
    schedulingFieldDetails: SchedulingFieldDetails;
}
