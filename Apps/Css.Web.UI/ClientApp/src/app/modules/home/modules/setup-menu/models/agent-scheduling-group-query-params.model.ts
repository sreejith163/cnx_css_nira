import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class AgentSchedulingGroupQueryParams extends QueryStringParameters {
    clientId: number;
    clientLobGroupId: number;
    skillGroupId: number;
    skillTagId: number;
}

