import { OperationHour } from './operation-hour.model';
import { SkillTagBase } from './skill-tag-base.model';

export class AddSkillTag extends SkillTagBase {
    operationHour: OperationHour[];
    createdBy: string;
}
