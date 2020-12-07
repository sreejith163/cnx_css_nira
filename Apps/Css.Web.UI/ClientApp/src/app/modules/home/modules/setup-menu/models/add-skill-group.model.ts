import { OperationHour } from './operation-hour.model';
import { SkillGroupBase } from './skill-group-base.model';

export class AddSkillGroup extends SkillGroupBase {
  createdBy: string;
  operationHour: OperationHour[];
}
