import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleRange } from './agent-schedule-range.model';

export class AgnetScheduleChartRange extends AgentScheduleRange {
    agentSchedulingGroupId: number;
    agentScheduleCharts: AgentScheduleChart;
}
