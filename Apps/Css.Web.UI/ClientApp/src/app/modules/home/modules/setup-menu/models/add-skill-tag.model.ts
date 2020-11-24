import { SkillTagBase } from './skill-tag-base.model';
import { SkillTagOperationHours } from './skill-tag-operation-hours.model';

export class AddSkillTag extends SkillTagBase {
    operationHour: SkillTagOperationHours[];
    createdBy: string;
}
