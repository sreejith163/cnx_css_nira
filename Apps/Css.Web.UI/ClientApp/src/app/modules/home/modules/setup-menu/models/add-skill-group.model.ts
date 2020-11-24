import { SkillGroupBase } from './skill-group-base.model';
import { SkillGroupOperationHour } from './skill-group-operation-hour.model';

export class AddSkillGroup extends SkillGroupBase {
  createdBy: string;
  operationHour: SkillGroupOperationHour[];
}
