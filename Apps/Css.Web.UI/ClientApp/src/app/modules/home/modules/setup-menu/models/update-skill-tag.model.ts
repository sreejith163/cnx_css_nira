import { SkillTagBase } from './skill-tag-base.model';
import { SkillTagOperationHours } from './skill-tag-operation-hours.model';

export class UpdateSkillTag extends SkillTagBase {
    operatingHours: SkillTagOperationHours[];
    modifiedBy: string;
}
