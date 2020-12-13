import { AgentSchedulingGroupBase } from './agent-scheduling-group-base.model';
import { AgentSchedulingGroupOperationHours } from './agent-scheduling-group-operation-hours.model';

export class AgentSchedulingGroupResponse extends AgentSchedulingGroupBase {
    operationHour: AgentSchedulingGroupOperationHours[];
}
