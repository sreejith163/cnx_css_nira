import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

export class SkillGroupQueryParameters extends QueryStringParameters {
    clientId?: number;
    clientLobGroupId?: number;
}
