import { SkillTagBase } from './skill-tag-base.model';
import { SkillTagOperationHours } from './skill-tag-operation-hours.model';

export class SkillTagResponse extends SkillTagBase {
    operationHour: SkillTagOperationHours[];
}
