import { AgentScheduleType } from '../enums/agent-schedule-type.enum';
import { AgentScheduleChart } from './agent-schedule-chart.model';
import { AgentScheduleManagerChart } from './agent-schedule-manager-chart.model';

export class UpdateAgentschedulechart {
    agentScheduleType: AgentScheduleType;
    agentScheduleCharts: AgentScheduleChart[];
    agentScheduleManagerCharts: AgentScheduleManagerChart[];
    modifiedBy: string;
}
