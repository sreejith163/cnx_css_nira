import { OperationHour } from './operation-hour.model';
import { SkillGroupBase } from './skill-group-base.model';

export class SkillGroupResponse extends SkillGroupBase {
    operationHour: OperationHour[];
}
