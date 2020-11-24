import { SkillGroupBase } from './skill-group-base.model';
import { SkillGroupOperationHour } from './skill-group-operation-hour.model';

export class UpdateSkillGroup extends SkillGroupBase {
    modifiedBy: string;
    operationHour: SkillGroupOperationHour[];
}
