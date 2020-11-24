import { SkillGroupBase } from './skill-group-base.model';
import { SkillTagOperationHours } from './skill-tag-operation-hours.model';

export class SkillGroupResponse extends SkillGroupBase {
    operationHour: SkillTagOperationHours[];
}
