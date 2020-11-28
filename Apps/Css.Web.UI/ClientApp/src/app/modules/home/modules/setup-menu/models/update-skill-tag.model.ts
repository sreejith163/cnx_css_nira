import { OperationHour } from './operation-hour.model';
import { SkillTagBase } from './skill-tag-base.model';

export class UpdateSkillTag extends SkillTagBase {
    operatingHours: OperationHour[];
    modifiedBy: string;
}
