import { QueryStringParameters } from "src/app/shared/models/query-string-parameters.model";

export class AgentCategoryValueQueryParameter extends QueryStringParameters {
    agentSchedulingGroupId?: number;
    agentCategoryId?: number;
}