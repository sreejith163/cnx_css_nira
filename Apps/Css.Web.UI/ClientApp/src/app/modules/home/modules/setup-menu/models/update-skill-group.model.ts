import { OperationHour } from './operation-hour.model';
import { SkillGroupBase } from './skill-group-base.model';

export class UpdateSkillGroup extends SkillGroupBase {
    modifiedBy: string;
    operationHour: OperationHour[];
}
