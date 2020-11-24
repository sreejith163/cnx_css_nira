import { SchedulingCodeType } from './scheduling-code-type.model';

export class SchedulingCodeAttributes {
     description: string;
     priorityNumber: number;
     schedulingTypeCode: SchedulingCodeType[];
     iconId: number;
}
