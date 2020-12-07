import { OperationHour } from './operation-hour.model';
import { SkillTagBase } from './skill-tag-base.model';

export class SkillTagResponse extends SkillTagBase {
    operationHour: OperationHour[];
}
